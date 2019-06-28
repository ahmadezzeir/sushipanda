using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto);

        Task ForgotPassword(string email);

        Task ChangeEmailAsync(ChangeEmailDto dto);

        Task ConfirmEmailAsync(string token);
    }
}
