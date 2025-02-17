using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieWorld.Api.Controllers;
using MovieWorld.Service;
using MovieWorld.Service.Models;

public class MovieControllerTests
{
    private readonly Mock<IMovieService> _mockMovieService;
    private readonly MovieController _controller;

    public MovieControllerTests()
    {
        _mockMovieService = new Mock<IMovieService>();
        _controller = new MovieController(_mockMovieService.Object);
    }

    [Fact]
    public async Task GetMovies_ShouldReturnMovies_WhenProvidersReturnMovies()
    {
        var movieDetailsList = new List<MovieDetails>
        {
            new MovieDetails { Title = "Movie1", Price = 10 },
            new MovieDetails { Title = "Movie2", Price = 15 }
        };

        _mockMovieService.Setup(service => service.GetMovies())
            .ReturnsAsync(movieDetailsList);

        var result = await _controller.GetMovies();

        Assert.Equal(2, result.Count);
    }
    [Fact]
    public async Task GetMovies_ShouldReturnEmptyList_WhenBothProvidersFailOrMoviesNotReturned()
    {
        _mockMovieService.Setup(service => service.GetMovies())
            .ReturnsAsync(new List<MovieDetails>());

        var result = await _controller.GetMovies();

        Assert.Equal(0, result.Count);
    }
}
