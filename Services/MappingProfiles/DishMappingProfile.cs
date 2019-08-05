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

            CreateMap<DishCreationDto, Dish>()
                .ForMember(dest => dest.FileId, opt => opt.MapFrom(x => x.ImgId));
        }
    }
}