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
    public class UsersService : ServiceBaseSql, IUsersService
    {
        private readonly UserManager<User> _userManager;

        public UsersService(IMapper mapper, IComponentContext scope, UserManager<User> userManager)
            : base(mapper, scope)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto)
        {
            var user = Mapper.Map<User>(userCreationDto);
            return await _userManager.CreateAsync(user, userCreationDto.Password);
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            }
        }
    }
}