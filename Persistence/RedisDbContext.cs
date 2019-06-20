using Domain.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Persistence
{
    public class RedisDbContext
    {
        public IDatabase Database { get; set; }

        public RedisDbContext(IOptions<RedisOptions> redisOptions)
        {
            var muxer = ConnectionMultiplexer.Connect(redisOptions.Value.ConnectionString);
            Database = muxer.GetDatabase();
        }
    }
}