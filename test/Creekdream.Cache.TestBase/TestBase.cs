using Microsoft.Extensions.DependencyInjection;
using System;

namespace Creekdream.Cache.TestBase
{
    /// <summary>
    /// Cache test base class
    /// </summary>
    public abstract class CacheTestBase
    {
        protected readonly IServiceProvider ServiceProvider;

        /// <inheritdoc />
        public CacheTestBase()
        {
            ServiceProvider = ConfigureServices(new ServiceCollection());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            return services.BuildServiceProvider();
        }
    }
}
