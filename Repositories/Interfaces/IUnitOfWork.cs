using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();

        IRepository<T> Repository<T>() where T : EntityBase;
    }
}