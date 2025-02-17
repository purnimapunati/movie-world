using Microsoft.AspNetCore.Mvc;
using MovieWorld.Service;
using MovieWorld.Service.Models;

namespace MovieWorld.Api.Controllers
{
    [Route("Movies"), ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public Task<IList<MovieDetails>> GetMovies()
        {
            return _movieService.GetMovies();
        }
    }
}


