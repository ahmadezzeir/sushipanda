using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Repositories.Interfaces;
using Services.Dtos;
using Services.Interfaces;

namespace Services
{
    public class UsersService : ServiceBase, IUsersService
    {
        private readonly UserManager<User> _userManager;

        public UsersService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<User> userManager)
            : base(mapper, unitOfWork)
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