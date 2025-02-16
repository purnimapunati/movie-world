using MovieWorld.Service.Models;

namespace MovieWorld.Service
{
    public interface IMovieService
    {
        public Task<IList<MovieDetails>> GetMovies();
    }
}
