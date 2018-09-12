using System;
using System.Collections.Generic;

namespace Creekdream.Cache
{
    /// <summary>
    /// Define the cache management interface,A cache manager should work as Singleton
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// Gets all caches.
        /// </summary>
        IReadOnlyList<ICache> GetAllCaches();

        /// <summary>
        /// Gets a cache instance, It may create the cache if it does not already exists.
        /// </summary>
        ICache GetCache(string name);
    }
}

