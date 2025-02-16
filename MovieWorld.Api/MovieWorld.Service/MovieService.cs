using AutoMapper;
using MovieWorld.Infra;
using MovieWorld.Service.Models;

namespace MovieWorld.Service
{
    public class MovieService : IMovieService
    {
        private readonly IApiClientFactory _apiClientFactory;
        private readonly IMapper _mapper;

        public MovieService(IApiClientFactory apiClientFactory, IMapper mapper)
        {
            _apiClientFactory = apiClientFactory;
            _mapper = mapper;
        }

        public async Task<IList<MovieDetails>> GetMovies()
        {
            var movies = await GetMoviesFromProviders();

            var cheapestMovies = movies
                                   .GroupBy(m => m.Title)
                                   .Select(g => g.OrderBy(m => m.Price).First())
                                   .ToList();
            return _mapper.Map<IList<MovieDetails>>(cheapestMovies);

        }
        private async Task<IList<MovieDetailsDto>> GetMoviesFromProviders()
        {
            var movies = new List<MovieDetailsDto>();
            foreach (MovieProviderType provider in Enum.GetValues(typeof(MovieProviderType)))
            {
                var movieIds = await GetMovieIds(provider);
                foreach (var id in movieIds)
                {
                    var movie = await GetMovieById(id, provider);
                    movies.Add(movie);
                }
            }
            return movies;
        }

        private async Task<List<string>> GetMovieIds(MovieProviderType movieProviderType)
        {
            var movies = await _apiClientFactory.Get<MovieList>(movieProviderType, "movies");
            return movies.Movies.Select(x => x.ID).ToList();
        }
        private async Task<MovieDetailsDto> GetMovieById(string Id, MovieProviderType movieProviderType)
        {
            return await _apiClientFactory.Get<MovieDetailsDto>(movieProviderType, $"movie/{Id}");
        }
    }
}

