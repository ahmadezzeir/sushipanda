using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Services.Identity
{
    public class UserStore : IUserPasswordStore<User>, IUserEmailStore<User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _usersRepository;

        public UserStore(IComponentContext context)
        {
            _unitOfWork = context.ResolveKeyed<IUnitOfWork>("sql");
            _usersRepository = _unitOfWork.Repository<User>();
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _usersRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await _usersRepository.Remove(user.Id);
            await _unitOfWork.CommitAsync();

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(userId, out var id))
            {
                return await _usersRepository.GetByIdAsync(id,
                    x => x.Include(u => u.UserRoles).ThenInclude(u => u.Role));
            }

            return await Task.FromResult((User)null);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _usersRepository.FindAsync(x => x.NormalizedName == normalizedUserName, 
                x => x.Include(u => u.UserRoles).ThenInclude(u => u.Role));
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Name = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _usersRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _usersRepository.FindAsync(x => x.NormalizedEmail == normalizedEmail);
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}