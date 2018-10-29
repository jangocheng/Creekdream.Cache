using System;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Cache.Redis
{
    /// <summary>
    /// Cache service extension
    /// </summary>
    public static class RedisCacheServiceCollectionExtensions
    {
        /// <summary>
        /// Add Redis Cache Service
        /// </summary>
        public static void AddCreekdreamRedisCache(this IServiceCollection services, Action<RedisCacheOptions> configOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configOptions == null)
            {
                throw new ArgumentNullException(nameof(configOptions));
            }
            services.AddOptions();
            services.Configure(configOptions);
            services.AddSingleton<ICacheManager, RedisCacheManager>();
        }
    }
}
