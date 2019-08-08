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
                .ForMember(dest => dest.ImgId, opt => opt.MapFrom(x => x.File.Id))
                .ForMember(dest => dest.ImgPath, opt => opt.MapFrom(x => x.File.Name))
                .ReverseMap();

            CreateMap<DishCreationDto, Dish>()
                .ForMember(dest => dest.FileId, opt => opt.MapFrom(x => x.ImgId));
        }
    }
}