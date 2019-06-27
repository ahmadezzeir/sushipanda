using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Services.Dtos;
using Services.Extensions;
using Services.Interfaces;

namespace Services
{
    public class UsersService : ServiceBaseSql, IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(IMapper mapper, IComponentContext scope, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor) : base(mapper, scope)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task ChangeEmailAsync(ChangeEmailDto dto)
        {
            var a = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(a);
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
            await _userManager.ChangeEmailAsync(user, dto.NewEmail, token);
        }
    }
}