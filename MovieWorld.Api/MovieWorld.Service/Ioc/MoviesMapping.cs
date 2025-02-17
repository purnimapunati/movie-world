using AutoMapper;
using MovieWorld.Infra.Models;
using MovieWorld.Service.Models;

namespace MovieWorld.Service.Ioc
{
    public class MoviesMapping : Profile
    {
        public MoviesMapping() {
            CreateMap<MovieDetailsDto, MovieDetails>();
        }
    }
}
