using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Creekdream.Cache.Memory
{
    /// <summary>
    /// Used to store cache in Memory.
    /// </summary>
    public class MemoryCache : CacheBase
    {
        private readonly string _name;
        private readonly MemoryCacheOptions _cacheOptions;
        private Microsoft.Extensions.Caching.Memory.MemoryCache _memoryCache;

        /// <inheritdoc />
        public MemoryCache(ILogger logger, string name, MemoryCacheOptions cacheOptions)
            : base(logger)
        {
            _name = name;
            _cacheOptions = cacheOptions;
            _memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(_cacheOptions);
        }

        /// <inheritdoc />
        public override async Task<T> GetAsync<T>(string key)
        {
            if (!_memoryCache.TryGetValue<T>(key, out T value))
            {
                return default(T);
            }
            return await Task.FromResult(value);
        }

        /// <inheritdoc />
        public override async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }
            _memoryCache.Set(key, value, slidingExpireTime ?? _cacheOptions.DefaultSlidingExpireTime);
            await Task.FromResult(0);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            await Task.FromResult(0);
        }

        /// <inheritdoc />
        public override async Task ClearAsync()
        {
            _memoryCache.Dispose();
            _memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(_cacheOptions);
            await Task.FromResult(0);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}