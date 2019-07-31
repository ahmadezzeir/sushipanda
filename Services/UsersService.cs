using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Services.Abstractions;
using Services.Dtos;
using Services.Events.EventHandling.Interfaces;
using Services.Events.Models;
using Services.Interfaces;

namespace Services
{
    public class UsersService : ServiceBaseSql, IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventsManager _eventsManager;

        public UsersService(IMapper mapper, IComponentContext scope, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, IEventsManager eventsManager) : base(mapper, scope)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _eventsManager = eventsManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto)
        {
            var user = Mapper.Map<User>(userCreationDto);
            var identityResult = await _userManager.CreateAsync(user, userCreationDto.Password);

            if (!identityResult.Errors.Any())
            {
                _eventsManager.EnqueueEvent(new UserRegisteredEvent
                {
                    Username = user.Name,
                    Email = user.Email,
                    Password = userCreationDto.Password,
                    Token = await _userManager.GenerateEmailConfirmationTokenAsync(user)
                });
            }

            return identityResult;
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                await _userManager.GeneratePasswordResetTokenAsync(user);
            }
        }

        public async Task ChangeEmailAsync(ChangeEmailDto dto)
        {
            var a = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(a);
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
            await _userManager.ChangeEmailAsync(user, dto.NewEmail, token); 
        }

        public async Task ConfirmEmailAsync(string token)
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            await _userManager.ConfirmEmailAsync(user, token);
        }
    }
}   