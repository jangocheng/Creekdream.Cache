using System;
using System.Threading.Tasks;

namespace Creekdream.Cache
{
    /// <summary>
    /// Defines a cache that can be store and get items by keys.
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Gets an item from the cache.
        /// This method hides cache provider failures (and logs them),
        /// uses the factory method to get the object if cache provider fails.
        /// </summary>
        Task<T> GetAsync<T>(string key, Func<string, T> factory = null);

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Set an item in the cache by a key.
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null);

        /// <summary>
        /// Removes a cache item by it's key.
        /// </summary>
        Task RemoveAsync(string key);

        /// <summary>
        /// Clears all items in this cache.
        /// </summary>
        Task ClearAsync();
    }
}

