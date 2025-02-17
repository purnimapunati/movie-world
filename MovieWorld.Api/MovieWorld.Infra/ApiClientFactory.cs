using Polly;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using MovieWorld.Infra.Models;

namespace MovieWorld.Infra
{
    public class ApiClientFactory : IApiClientFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiClientFactory> _logger;
        private const int MaxRetries = 3;
        private const int RetryDelayMilliseconds = 1000;

        public ApiClientFactory(IConfiguration configuration, ILogger<ApiClientFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T> Get<T>(MovieProviderType provider, string endPoint)
        {
            var baseUrl = GetBaseUrl(provider);
            var url = $"{baseUrl}/{endPoint}";
            var accessToken = _configuration["x-access-token"];

            using var client = GetHttpClient(accessToken, baseUrl);

            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(MaxRetries, retryAttempt =>
                    TimeSpan.FromMilliseconds(RetryDelayMilliseconds * Math.Pow(2, retryAttempt))
                );

            HttpResponseMessage response = null;
            string content = string.Empty;

            try
            {
                response = await retryPolicy.ExecuteAsync(async () =>
                {
                    response = await client.GetAsync(url);
                    content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }

                    _logger.LogWarning($"Failed to fetch {endPoint} (Status: {response.StatusCode}, Reason: {response.ReasonPhrase})");
                    return response;
                });

                return ValidateResponse<T>(response, content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching {endPoint}: {ex.Message}");
                throw new HttpRequestException($"Failed to fetch data from {url} after {MaxRetries} attempts.", ex);
            }
        }

        private string GetBaseUrl(MovieProviderType provider)
        {
            return _configuration[$"MovieProviderUrls:{provider}"];
        }

        private HttpClient GetHttpClient(string accessToken, string url)
        {
            var handler = new HttpClientHandler
            {
                PreAuthenticate = true
            };

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(url),
                Timeout = TimeSpan.FromMinutes(1)
            };

            client.DefaultRequestHeaders.Add("x-access-token", accessToken);

            return client;
        }

        private T ValidateResponse<T>(HttpResponseMessage message, string content)
        {
            try
            {
                if (message.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorMessage = JsonConvert.DeserializeObject<List<string>>(content);
                    var messages = errorMessage.Select(x => x);
                    return (T)Convert.ChangeType(string.Join(", ", messages.ToArray()), typeof(T));
                }

                if (typeof(T).Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                {
                    return (T)Convert.ChangeType(string.Join(", ", content), typeof(T));
                }

                message.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Error deserializing response to type {typeof(T).Name}. Raw JSON: {content}");
                _logger.LogCritical($"Response Reason Phrase: {message.ReasonPhrase}");

                throw new JsonSerializationException($"Error deserializing response: {e.Message}", e);
            }
        }
    }
}
