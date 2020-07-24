using AutoMapper;
using SkillsTest.Domain.Entities;
using SkillsTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsTest.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieBindingModel>();
            CreateMap<MovieBindingModel, Movie>();
        }
    }
}
