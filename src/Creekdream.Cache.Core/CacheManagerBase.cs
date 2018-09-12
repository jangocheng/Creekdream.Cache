using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Creekdream.Cache
{
    /// <summary>
    /// Base class for management caches.
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        /// <inheritdoc />
        public CacheManagerBase()
        {
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        /// <inheritdoc />
        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        /// <inheritdoc />
        public ICache GetCache(string name)
        {
            return Caches.GetOrAdd(name, (cacheName) =>
            {
                return CreateCache(cacheName);
            });
        }

        /// <inheritdoc />
        protected abstract ICache CreateCache(string name);

        /// <inheritdoc />
        public virtual void Dispose()
        {
            foreach (var cache in Caches.Values)
            {
                cache.Dispose();
            }
            Caches.Clear();
        }
    }
}

