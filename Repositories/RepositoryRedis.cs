using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using StackExchange.Redis;
using Persistence;
using Repositories.Extensions;
using Repositories.Interfaces;
using Newtonsoft.Json;

namespace Repositories
{
    public class RepositoryRedis<T> : IRepository<T> where T : EntityBase
    {
        private readonly IDatabase _db;
        private readonly RedisDbContext _redisContext;

        public RepositoryRedis(RedisDbContext redisContext)
        {
            _redisContext = redisContext;
            _db = redisContext.Database;
        }

        public async Task<T> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var redisValue = await _db.StringGetAsync(id.ToString());

            return JsonConvert.DeserializeObject<T>(redisValue.ToString());
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>> filterPredicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(T entity)
        {
            await _redisContext.AddCommandAsync(async () =>
            {
                dynamic obj = new ExpandoObject();
                var properties = entity.GetType().GetProperties().Where(x => x.Name != nameof(entity.Id));
                foreach (var prop in properties)
                {
                    AddProperty(obj, prop.Name, prop.GetValue(entity));
                }

                var jsonObj = JsonConvert.SerializeObject(obj);

                await _db.StringSetAsync(entity.Id.ToString(), jsonObj);
            });
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(Guid id)
        {
            await _redisContext.AddCommandAsync(async () =>
            {
                await _db.KeyDeleteAsync(id.ToString());
            });
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}
