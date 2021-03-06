﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Persistence;
using Repositories.Extensions;
using Repositories.Interfaces;
using StackExchange.Redis;

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

#pragma warning disable 1998
        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
#pragma warning restore 1998
        {
            throw new NotImplementedException();
        }

#pragma warning disable 1998
        public async Task<PagedResult<T>> GetPagedAsync(int page,
#pragma warning restore 1998
            int pageSize,
            Expression<Func<T, bool>> filterPredicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            throw new NotImplementedException();
        }

#pragma warning disable 1998
        public async Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> predicate)
#pragma warning restore 1998
        {
            throw new NotImplementedException();
        }

#pragma warning disable 1998
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
#pragma warning restore 1998
        {
            throw new NotImplementedException();
        }

#pragma warning disable 1998
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
#pragma warning restore 1998
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
