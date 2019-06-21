using System.Security.Authentication;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Services.Dtos;
using Services.Interfaces;

namespace Services
{
    public class AuthService : ServiceBaseSql, IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper, IComponentContext context, UserManager<User> userManager) 
            : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<LoggedInUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Mapper.Map<LoggedInUserDto>(user);
            }

            throw new InvalidCredentialException("Entered credentials are wrong");
        }

        public async Task<LoggedInUserDto> GetUserInfoAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return Mapper.Map<LoggedInUserDto>(user);
        }
    }
}