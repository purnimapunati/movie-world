using MovieWorld.Service.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace MovieWorld.Infra
{
    public class ApiClientFactory : IApiClientFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiClientFactory> _logger;
        private const int MaxRetries = 3;
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
            var retries = 1;

            using var client = GetHttpClient(accessToken, baseUrl);

            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            while (!response.IsSuccessStatusCode && retries <= MaxRetries)
            {
                _logger.LogCritical($"Failed to fetch {endPoint} details and Retrying for {MaxRetries} time.");
                response = await client.GetAsync(url);
                content = await response.Content.ReadAsStringAsync();
                retries++;
            }

            return ValidateResponse<T>(response, content);
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
                Timeout = TimeSpan.FromMinutes(10)
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
                    var jsonObject = JsonConvert.DeserializeObject<List<string>>(content);
                    var messages = jsonObject.Select(x => x);
                    return (T)Convert.ChangeType(string.Join(", ", messages.ToArray()), typeof(T));
                }

                else
                {
                    if (typeof(T).Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)Convert.ChangeType(string.Join(", ", content), typeof(T));
                    }
                    else
                    {
                        message.EnsureSuccessStatusCode();
                        var jsonObject = JsonConvert.DeserializeObject<T>(content);
                        return jsonObject;
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogCritical($"Issue Deserializing JSON to C# object {typeof(T).Name} RAW JSON: {content}");
                _logger.LogCritical($"Response Reason Phrase: {message.ReasonPhrase}");

                throw;
            }
        }

    }
}
