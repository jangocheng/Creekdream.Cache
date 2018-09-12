using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Creekdream.Cache.Redis
{
    /// <summary>
    /// Redis cache management instance
    /// </summary>
    public class RedisCacheManager : CacheManagerBase
    {
        private readonly ILogger _logger;
        private readonly RedisCacheOptions _cacheOptions;
        private readonly ConnectionMultiplexer _redisConnections;
        private readonly IDatabase _database;

        /// <inheritdoc />
        public RedisCacheManager(ILogger<RedisCache> logger, IOptions<RedisCacheOptions> cacheOptions)
        {
            _logger = logger;
            _cacheOptions = cacheOptions.Value;
            _redisConnections = ConnectionMultiplexer.Connect(_cacheOptions.ConnectionString);
            _database = _redisConnections.GetDatabase();
        }

        /// <inheritdoc />
        protected override ICache CreateCache(string name)
        {
            return new RedisCache(_logger, name, _cacheOptions.DefaultSlidingExpireTime, _database);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            _redisConnections.Close();
            _redisConnections.Dispose();
        }
    }
}

