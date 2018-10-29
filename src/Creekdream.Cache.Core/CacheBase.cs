using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

namespace Creekdream.Cache
{
    /// <summary>
    /// Base class for caches.
    /// </summary>
    public abstract class CacheBase : ICache
    {
        private readonly AsyncLock _asyncLock = new AsyncLock();
        private readonly ILogger _logger;

        /// <inheritdoc />
        protected CacheBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync<T>(string key, Func<string, T> factory = null)
        {
            T item = default(T);
            try
            {
                item = await GetAsync<T>(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get cache error");
            }
            if (item == null && factory != null)
            {
                using (await _asyncLock.LockAsync())
                {
                    try
                    {
                        item = await GetAsync<T>(key);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Get cache error");
                    }

                    if (item == null)
                    {
                        item = factory(key);

                        if (item == null)
                        {
                            return default(T);
                        }

                        try
                        {
                            await SetAsync(key, item);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Set cache error");
                        }
                    }
                }
            }

            return item;
        }

        /// <inheritdoc />
        public abstract Task<T> GetAsync<T>(string key);

        /// <inheritdoc />
        public abstract Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null);

        /// <inheritdoc />
        public abstract Task RemoveAsync(string key);

        /// <inheritdoc />
        public abstract Task ClearAsync();

        /// <inheritdoc />
        public virtual void Dispose()
        {

        }
    }
}

