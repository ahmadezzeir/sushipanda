using System.Linq;
using AutoMapper;
using Domain.Models;
using Services.Dtos;

namespace Services.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserCreationDto>()
                .ReverseMap();

            CreateMap<User, LoggedInUserDto>()
                .ForMember(x => x.Roles, 
                    opt => opt.MapFrom(x => x.UserRoles.Select(ur => ur.Role.Name)));

        }
    }
}