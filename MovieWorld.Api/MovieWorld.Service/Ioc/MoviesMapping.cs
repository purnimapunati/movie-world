﻿using AutoMapper;
using MovieWorld.Service.Models;
using System.Net;

namespace MovieWorld.Service.Ioc
{
    public class MoviesMapping : Profile
    {
        public MoviesMapping() {
            CreateMap<MovieDetailsDto, MovieDetails>();
        }
    }
}
