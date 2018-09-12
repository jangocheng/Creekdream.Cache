using System;

namespace Creekdream.Cache.Memory
{
    /// <summary>
    /// Memory cache options
    /// </summary>
    public class MemoryCacheOptions : Microsoft.Extensions.Caching.Memory.MemoryCacheOptions
    {
        /// <summary>
        /// Cache slip expiration time(default 1 hour)
        /// </summary>
        public TimeSpan DefaultSlidingExpireTime { get; set; } = TimeSpan.FromHours(1);
    }
}

