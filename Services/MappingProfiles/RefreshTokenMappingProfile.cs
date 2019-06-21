using AutoMapper;
using Domain.Models;
using Services.Dtos;

namespace Services.MappingProfiles
{
    public class RefreshTokenMappingProfile : Profile
    {
        public RefreshTokenMappingProfile()
        {
            CreateMap<RefreshToken, RefreshTokenDto>()
                .ReverseMap();
        }
    }
}