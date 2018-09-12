using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Creekdream.Cache.Memory
{
    /// <summary>
    /// Memory cache management instance
    /// </summary>
    public class MemoryCacheManager : CacheManagerBase
    {
        private readonly ILogger _logger;
        private readonly MemoryCacheOptions _cacheOptions;

        /// <inheritdoc />
        public MemoryCacheManager(IOptions<MemoryCacheOptions> cacheOptions, ILogger<MemoryCache> logger)
        {
            _logger = logger;
            _cacheOptions = cacheOptions.Value;
        }

        /// <inheritdoc />
        protected override ICache CreateCache(string name)
        {
            return new MemoryCache(_logger, name, _cacheOptions);
        }
    }
}

