using AutoMapper;
using Domain.Models;
using Services.Dtos;

namespace Services.MappingProfiles
{
    public class DishMappingProfile : Profile
    {
        public DishMappingProfile()
        {
            CreateMap<Dish, DishDto>()
                .ReverseMap();
        }
    }
}