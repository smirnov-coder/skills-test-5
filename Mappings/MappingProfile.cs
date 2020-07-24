using AutoMapper;
using SkillsTest.Domain.Entities;
using SkillsTest.Models;

namespace SkillsTest.Mappings
{
    /// <summary>
    /// Маппинг Automapper.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieBindingModel>();
            CreateMap<MovieBindingModel, Movie>();
        }
    }
}
