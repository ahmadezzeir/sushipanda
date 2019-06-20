using System;
using System.Threading.Tasks;
using Domain.Models;
using Repositories.Interfaces;

namespace Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public RefreshTokenService(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> GetRefreshTokenByIdAsync(string userId)
        {
            var token = await _refreshTokenRepository.GetByIdAsync(Guid.Parse(userId));
            return token.Value;
        }

        public async Task CreateRefreshTokenAsync()
        {

        }

        public async Task DeleteRefreshTokenAsync()
        {

        }
    }

    public interface IRefreshTokenService
    {
        Task<string> GetRefreshTokenByIdAsync(string userId);
    }
}
