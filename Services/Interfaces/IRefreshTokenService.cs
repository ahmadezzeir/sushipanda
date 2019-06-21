using System;
using System.Threading.Tasks;
using Services.Dtos;

namespace Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task CreateRefreshTokenAsync(RefreshTokenDto tokenDto);

        Task<RefreshTokenDto> GetRefreshTokenByIdAsync(string userId);

        Task DeleteRefreshTokenAsync(Guid id);
    }
}