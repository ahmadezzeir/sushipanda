using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto);

        Task ForgotPassword(string email);
    }
}
