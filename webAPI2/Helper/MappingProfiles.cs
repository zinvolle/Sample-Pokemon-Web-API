using AutoMapper;
using webAPI2.DTO;
using webAPI2.Models;

namespace webAPI2.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}
