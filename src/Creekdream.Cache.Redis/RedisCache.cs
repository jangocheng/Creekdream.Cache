using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Creekdream.Cache.Redis
{
    /// <summary>
    /// Used to store cache in a Redis server.
    /// </summary>
    public class RedisCache : CacheBase
    {
        private readonly string _name;
        private readonly TimeSpan _defaultSlidingExpireTime;
        private readonly IDatabase _database;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <inheritdoc />
        public RedisCache(ILogger logger, string name, TimeSpan defaultSlidingExpireTime, IDatabase database)
            : base(logger)
        {
            _name = name;
            _defaultSlidingExpireTime = defaultSlidingExpireTime;
            _database = database;
            _jsonSerializerSettings =
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
        }

        /// <inheritdoc />
        public override async Task<T> GetAsync<T>(string key)
        {
            key = GetLocalizedKey(key);
            var redisObject = await _database.StringGetAsync(key);
            if (redisObject.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(redisObject, _jsonSerializerSettings);
            }
            return default(T);
        }

        /// <inheritdoc />
        public override async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null)
        {
            key = GetLocalizedKey(key);
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }

            await _database.StringSetAsync(
                key,
                JsonConvert.SerializeObject(
                    value,
                    Formatting.Indented,
                    _jsonSerializerSettings),
                slidingExpireTime ?? _defaultSlidingExpireTime);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(string key)
        {
            key = GetLocalizedKey(key);
            await _database.KeyDeleteAsync(key);
        }

        /// <inheritdoc />
        public override async Task ClearAsync()
        {
            await _database.ScriptEvaluateAsync(@"
                local keys = redis.call('keys', ARGV[1]) 
                for i=1,#keys,5000 do 
                redis.call('del', unpack(keys, i, math.min(i+4999, #keys)))
                end", values: new RedisValue[] { GetLocalizedKey("*") });
        }

        /// <inheritdoc />
        protected virtual string GetLocalizedKey(string key)
        {
            return "n:" + _name + ",c:" + key;
        }
    }
}

