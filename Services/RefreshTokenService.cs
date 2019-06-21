using System;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Domain.Models;
using Repositories.Interfaces;
using Services.Abstractions;
using Services.Dtos;
using Services.Interfaces;

namespace Services
{
    public class RefreshTokenService : ServiceBaseRedis, IRefreshTokenService
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public RefreshTokenService(IMapper mapper, IComponentContext scope) : base(mapper, scope)
        {
            _refreshTokenRepository = UnitOfWork.Repository<RefreshToken>();
        }

        public async Task CreateRefreshTokenAsync(RefreshTokenDto tokenDto)
        {
            var token = Mapper.Map<RefreshToken>(tokenDto);
            await _refreshTokenRepository.AddAsync(token);
            await UnitOfWork.CommitAsync();
        }

        public async Task<RefreshTokenDto> GetRefreshTokenByIdAsync(string userId)
        {
            var token = await _refreshTokenRepository.GetByIdAsync(Guid.Parse(userId));
            return Mapper.Map<RefreshTokenDto>(token);
        }

        public async Task DeleteRefreshTokenAsync(Guid id)
        {
            await _refreshTokenRepository.Remove(id);
            await UnitOfWork.CommitAsync();
        }
    }
}