using Creekdream.Cache.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Creekdream.Cache.Memory.Tests
{
    /// <summary>
    /// Memory cache unit test
    /// </summary>
    public class MemoryCacheTest : CacheTestBase
    {
        private readonly ICache _cache;

        /// <inheritdoc />
        public MemoryCacheTest() : base()
        {
            var cacheManager = ServiceProvider.GetService<ICacheManager>();
            _cache = cacheManager.GetCache(nameof(MemoryCacheTest));
        }

        /// <inheritdoc />
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCreekdreamMemoryCache(options => { });
            return base.ConfigureServices(services);
        }

        /// <summary>
        /// Simple access expiration test
        /// </summary>
        [Fact]
        public async Task Simple_Get_Set_Expire_Test()
        {
            var key = "TestKey";
            var value = "TestValue";
            await _cache.SetAsync(key, value, TimeSpan.FromMilliseconds(100));
            var strValue = await _cache.GetAsync<string>(key);
            strValue.ShouldBe(value);

            await Task.Delay(150);

            strValue = await _cache.GetAsync<string>(key);
            strValue.ShouldBeNull();
        }

        /// <summary>
        /// Object access test
        /// </summary>
        [Fact]
        public async Task Object_Get_Set_Test()
        {
            var key = nameof(ObjectTestClass);
            var value = new ObjectTestClass()
            {
                Name = "zhangsan",
                Age = 18
            };
            await _cache.SetAsync(key, value);

            var objValue = await _cache.GetAsync<ObjectTestClass>(key);
            objValue.Name.ShouldBe(value.Name);
            objValue.Age.ShouldBe(value.Age);
        }

        /// <summary>
        /// Delete cache key test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Remove_Clear_Key_Test()
        {
            var key = nameof(ObjectTestClass);
            var value = new ObjectTestClass()
            {
                Name = "zhangsan",
                Age = 18
            };

            // remove test
            await _cache.SetAsync(key, value);
            await _cache.RemoveAsync(key);
            var removeValue = await _cache.GetAsync<ObjectTestClass>(key);
            removeValue.ShouldBeNull();

            // clear test
            await _cache.SetAsync(key, value);
            await _cache.ClearAsync();
            var clearValue = await _cache.GetAsync<ObjectTestClass>(key);
            clearValue.ShouldBeNull();
        }
    }
}
