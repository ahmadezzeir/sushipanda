using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Extensions;

namespace Repositories.Interfaces
{
    public interface IRepository<TModel> where TModel : EntityBase
    {
        Task<TModel> GetByIdAsync(Guid id, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<PagedResult<TModel>> GetPagedAsync(int page, int pageSize,
            Expression<Func<TModel, bool>> filterPredicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null);

        Task<IEnumerable<TModel>> FindManyAsync(Expression<Func<TModel, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TModel, bool>> predicate);

        Task<TModel> FindAsync(Expression<Func<TModel, bool>> predicate, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null);

        Task AddAsync(TModel entity);

        void Update(TModel entity);

        void Remove(TModel entity);
    }
}