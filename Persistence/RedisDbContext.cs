using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Persistence
{
    public class RedisDbContext : IDisposable
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly List<Func<Task>> _commands;

        public IDatabase Database { get; set; }

        public RedisDbContext(IOptions<RedisOptions> redisOptions)
        {
            _multiplexer = ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString);
            Database = _multiplexer.GetDatabase();

            _commands = new List<Func<Task>>();
        }

        public Task AddCommandAsync(Func<Task> func)
        {
            _commands.Add(func);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            var transaction = Database.CreateTransaction();

            try
            {
                foreach (var command in _commands)
                {
                    await command();
                }

                var count = _commands.Count;
                return await transaction.ExecuteAsync() ? count : 0;
            }
            catch
            {
                // TODO: add logging
                return 0;
            }
        }

        public void Dispose()
        {
            _multiplexer.Dispose();
        }
    }
}