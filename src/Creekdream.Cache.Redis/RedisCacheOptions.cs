using Microsoft.Extensions.Options;
using System;

namespace Creekdream.Cache.Redis
{
    /// <summary>
    /// Redis cache options
    /// </summary>
    public class RedisCacheOptions : IOptions<RedisCacheOptions>
    {
        /// <summary>
        /// Redis connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Cache slip expiration time(default 1 hour)
        /// </summary>
        public TimeSpan DefaultSlidingExpireTime { get; set; } = TimeSpan.FromHours(1);

        public RedisCacheOptions Value => this;
    }
}

