using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MovieWorld.Infra;
using MovieWorld.Service;
using MovieWorld.Infra.Models;

public class MovieServiceTests
{
    private readonly Mock<IApiClientFactory> _mockApiClientFactory;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<MovieService>> _mockLogger;
    private readonly MovieService _movieService;

    public MovieServiceTests()
    {
        _mockApiClientFactory = new Mock<IApiClientFactory>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MovieService>>();
        _movieService = new MovieService(_mockApiClientFactory.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetMovieIds_ShouldReturnMovieIds_WhenProvidersReturnValidData()
    {
        _mockApiClientFactory.Setup(factory => factory.Get<MovieList>(It.IsAny<MovieProviderType>(), It.IsAny<string>()))
            .ReturnsAsync(new MovieList
            {
                Movies = new List<MovieSummaryDto>
        {
            new MovieSummaryDto { Title = "Movie1", Poster = "Poster1", ID = "1" },
            new MovieSummaryDto { Title = "Movie2", Poster = "Poster2", ID = "2" }
        }
            });

        var movieIds = await _movieService.GetMovieIds(MovieProviderType.Cinemaworld);


        Assert.NotNull(movieIds);
        Assert.Equal(2, movieIds.Count);
        Assert.Contains("1", movieIds);
        Assert.Contains("2", movieIds);
    }

    [Fact]
    public async Task GetMovieIds_ShouldReturnEmptyList_WhenProviderReturnsEmptyData()
    {
        _mockApiClientFactory.Setup(factory => factory.Get<MovieList>(It.IsAny<MovieProviderType>(), It.IsAny<string>()))
            .ReturnsAsync(new MovieList { Movies = new List<MovieSummaryDto>() });

        var movieIds = await _movieService.GetMovieIds(MovieProviderType.Cinemaworld);

        Assert.NotNull(movieIds);
        Assert.Empty(movieIds);
    }
    [Fact]

    public async Task GetMovieById_ShouldReturnMovieDetails_WhenProviderReturnsValidData()
    {
        _mockApiClientFactory.Setup(factory => factory.Get<MovieDetailsDto>(It.IsAny<MovieProviderType>(), It.IsAny<string>()))
            .ReturnsAsync(
            new MovieDetailsDto
            { Title = "Movie1", Price = 10, Rating = "6", Poster = "Poster1", ID = "1" });

        var movie = await _movieService.GetMovieById("1", MovieProviderType.Cinemaworld);

        Assert.NotNull(movie);
    }
    [Fact]
    public async Task GetMovieById_ShouldReturnMovieDetails_WhenProviderReturnsEmptyData()
    {
        _mockApiClientFactory.Setup(factory => factory.Get<MovieDetailsDto>(It.IsAny<MovieProviderType>(), It.IsAny<string>()))
            .ReturnsAsync(
            new MovieDetailsDto());

        var movie = await _movieService.GetMovieById("1", MovieProviderType.Cinemaworld);

        Assert.NotNull(movie);
    }
    [Fact]
    public async Task GetMovieFromProviders_ShouldReturnMovieDetails_WhenProvidersReturnsValidData()
    {
        _mockApiClientFactory.Setup(factory => factory.Get<MovieDetailsDto>(It.IsAny<MovieProviderType>(), It.IsAny<string>()))
            .ReturnsAsync(
            new MovieDetailsDto());

        var movie = await _movieService.GetMovieById("1", MovieProviderType.Cinemaworld);

        Assert.NotNull(movie);
    }
}
