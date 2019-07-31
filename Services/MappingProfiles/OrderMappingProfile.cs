using AutoMapper;
using Domain.Models;
using Services.Dtos;

namespace Services.MappingProfiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderCreationDto, Order>()
                .ForMember(x => x.Dishes, opt => opt.Ignore());
        }
    }
}