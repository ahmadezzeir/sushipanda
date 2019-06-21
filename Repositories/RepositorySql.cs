using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Extensions;
using Repositories.Interfaces;

namespace Repositories
{
    public class RepositorySql<TModel> : IRepository<TModel> where TModel : EntityBase
    {
        private readonly DbSet<TModel> _dbSet;

        public RepositorySql(DbContext context)
        {
            _dbSet = context.Set<TModel>();
        }

        public async Task<TModel> GetByIdAsync(Guid id, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null)
        {
            IQueryable<TModel> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }


            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<PagedResult<TModel>> GetPagedAsync(int page, int pageSize,
            Expression<Func<TModel, bool>> filterPredicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null)
        {
            IQueryable<TModel> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            if (filterPredicate != null)
            {
                query = query.Where(filterPredicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.GetPaged(page, pageSize);
        }

        public async Task<IEnumerable<TModel>> FindManyAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TModel> FindAsync(Expression<Func<TModel, bool>> predicate, Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null)
        {
            IQueryable<TModel> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public async Task<TModel> FindFirstAsync(Expression<Func<TModel, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TModel entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TModel entity)
        {
            _dbSet.Update(entity);
        }

        public async Task Remove(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
        }
    }
}