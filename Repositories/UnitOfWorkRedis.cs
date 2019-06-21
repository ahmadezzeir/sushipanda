using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Persistence;
using Repositories.Interfaces;

namespace Repositories
{
    public class UnitOfWorkRedis : IUnitOfWork
    {
        private readonly RedisDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private bool _disposed;

        public UnitOfWorkRedis(RedisDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public IRepository<T> Repository<T>() where T : EntityBase
        {
            if (_repositories.Keys.Contains(typeof(T)))
            {
                return _repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repo = new RepositoryRedis<T>(_dbContext);
            _repositories.Add(typeof(T), repo);

            return repo;
        }

        #region dispose
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                _disposed = true;
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