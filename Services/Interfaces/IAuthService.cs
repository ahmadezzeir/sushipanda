using System.Threading.Tasks;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoggedInUserDto> LoginAsync(LoginDto loginDto);

        Task<LoggedInUserDto> GetUserInfoAsync(string userId);
    }
}