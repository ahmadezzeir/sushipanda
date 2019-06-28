using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Emails;
using Emails.ViewModels;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Services.Dtos;
using Services.Interfaces;

namespace Services
{
    public class UsersService : ServiceBaseSql, IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailSenderService _mailSenderService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public UsersService(IMapper mapper, IComponentContext scope, UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor, IMailSenderService mailSenderService, 
            IRazorViewToStringRenderer razorViewToStringRenderer) : base(mapper, scope)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mailSenderService = mailSenderService;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto)
        {
            var user = Mapper.Map<User>(userCreationDto);
            var identityResult = await _userManager.CreateAsync(user, userCreationDto.Password);

            var emailView = new ConfirmMailboxEmailViewModel
            {
                Token = await _userManager.GenerateEmailConfirmationTokenAsync(user),
                Username = user.Name
            };
            var message = new MailMessage
            {
                BodyHtml = await _razorViewToStringRenderer.RenderViewToStringAsync("ConfirmMailboxEmailView", emailView),
                From = "admin@example.com",
                To = user.Email,
                Subject = "Thank you for registration"
            };
            await _mailSenderService.SendEmailAsync(message);

            return identityResult;
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

        public async Task ConfirmEmailAsync(string token)
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var res = await _userManager.ConfirmEmailAsync(user, token);
        }
    }
}