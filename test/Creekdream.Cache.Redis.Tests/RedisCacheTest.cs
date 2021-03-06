﻿using Creekdream.Cache.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Creekdream.Cache.Redis.Tests
{
    /// <summary>
    /// Redis cache unit test
    /// </summary>
    public class RedisCacheTest : CacheTestBase
    {
        private readonly ICache _cache;

        /// <inheritdoc />
        public RedisCacheTest()
        {
            var cacheManager = ServiceProvider.GetService<ICacheManager>();
            _cache = cacheManager.GetCache(nameof(RedisCacheTest));
        }

        /// <inheritdoc />
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCreekdreamRedisCache(
                options =>
                {
                    options.ConnectionString = "127.0.0.1";
                });
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
        public async Task Object_GetFactory_Test()
        {
            var key = nameof(ObjectTestClass);
            await _cache.RemoveAsync(key);
            var name = "zhangsan";
            var age = 18;
            await _cache.GetAsync(
                nameof(ObjectTestClass),
                cacheKey =>
                {
                    return new ObjectTestClass()
                    {
                        Name = name,
                        Age = age
                    };
                });
            var objValue = await _cache.GetAsync<ObjectTestClass>(key);
            objValue.ShouldNotBeNull();
            objValue.Name.ShouldBe(name);
            objValue.Age.ShouldBe(age);
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
