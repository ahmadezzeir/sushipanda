using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Repositories.Interfaces;

namespace Repositories
{
    public class RoleStore : IRoleStore<Role>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Role> _rolesRepository;

        public RoleStore(IUnitOfWork unitOfWork, IRepository<Role> rolesRepository)
        {
            _unitOfWork = unitOfWork;
            _rolesRepository = rolesRepository;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            await _rolesRepository.AddAsync(role);
            await _unitOfWork.CommitAsync();

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            _rolesRepository.Remove(role);
            await _unitOfWork.CommitAsync();

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(roleId, out var id))
            {
                return await _rolesRepository.GetByIdAsync(id);
            }

            return await Task.FromResult((Role)null);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _rolesRepository.FindAsync(x => x.Name == normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            _rolesRepository.Update(role);
            await _unitOfWork.CommitAsync();

            return await Task.FromResult(IdentityResult.Success);
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}