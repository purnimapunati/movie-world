using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieWorld.Infra;
using MovieWorld.Service.Models;
using MovieWorld.Infra.Models;

namespace MovieWorld.Service
{
    public class MovieService : IMovieService
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieService> _logger;
        private readonly Dictionary<string, CacheItem> _movieCache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public MovieService(IApiClientFactory apiClientFactory, IMapper mapper, ILogger<MovieService> logger)
        {
            _apiClientFactory = apiClientFactory;
            _mapper = mapper;
            _logger = logger;
            _movieCache = new Dictionary<string, CacheItem>();
        }

        public async Task<IList<MovieDetails>> GetMovies()
        {
            try
            {
                var movies = await GetMoviesFromProviders();
                var cheapestMovies = movies
                                    .Where(m => m != null)  
                                    .GroupBy(m => m.Title)
                                    .Select(g => g.OrderBy(m => m.Price).First())
                                    .ToList();

                return _mapper.Map<IList<MovieDetails>>(cheapestMovies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movies.");
                throw;
            }
        }

        private async Task<IList<MovieDetailsDto>> GetMoviesFromProviders()
        {
            var movies = new List<MovieDetailsDto>();

            try
            {
                var tasks = Enum.GetValues(typeof(MovieProviderType))
                                .Cast<MovieProviderType>()
                                .Select(provider => GetMoviesForProvider(provider));

                var providerMovies = await Task.WhenAll(tasks);

                foreach (var movieList in providerMovies)
                {
                    movies.AddRange(movieList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movies from providers.");
                throw;
            }

            return movies;
        }

        private async Task<IList<MovieDetailsDto>> GetMoviesForProvider(MovieProviderType provider)
        {
            try
            {
                _logger.LogInformation($"Fetching movies from provider: {provider}");
                var movieIds = await GetMovieIds(provider);
                var movieTasks = movieIds.Select(id => GetMovieById(id, provider));
                return await Task.WhenAll(movieTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movies from provider: {Provider}", provider);
                return new List<MovieDetailsDto>();
            }
        }

        private async Task<List<string>> GetMovieIds(MovieProviderType movieProviderType)
        {
            try
            {
                _logger.LogInformation("Fetching movie IDs for provider: {Provider}", movieProviderType);
                var movies = await _apiClientFactory.Get<MovieList>(movieProviderType, "movies");
                return movies.Movies.Select(x => x.ID).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movie IDs from provider: {Provider}", movieProviderType);
                return new List<string>();
            }
        }

        private async Task<MovieDetailsDto> GetMovieById(string Id, MovieProviderType movieProviderType)
        {
            if (_movieCache.ContainsKey(Id) && !_movieCache[Id].IsExpired(_cacheExpiration))
            {
                _logger.LogInformation("Returning cached movie details for ID: {Id}", Id);
                return _movieCache[Id].MovieDetails;
            }

            try
            {
                _logger.LogInformation("Fetching movie details for movie ID: {Id} from provider: {Provider}", Id, movieProviderType);
                var movieDetails = await _apiClientFactory.Get<MovieDetailsDto>(movieProviderType, $"movie/{Id}");

                if (movieDetails != null)
                {
                    _movieCache[Id] = new CacheItem
                    {
                        MovieDetails = movieDetails,
                        CachedAt = DateTime.UtcNow
                    };
                }

                return movieDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movie details for ID: {Id} from provider: {Provider}", Id, movieProviderType);
                return null;
            }
        }
    }
}