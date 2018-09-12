# Creekdream 缓存组件

本项目缓存组件适用用于.NET STANDARD 2.0 的项目，目前支持 Redis 缓存以及 Memory 缓存。

## 一、Creekdream Memory 缓存

内存缓存。

### 安装

``` csharp
Install-Package Creekdream.Cache.Memory
```

### 配置

``` csharp
services.AddMemoryCache(options => { });
```

## 二、Creekdream Redis 缓存

Redis 缓存在使用过程中，请尽量不要存储内容过大，可能会造成超时等问题。

### 安装

``` csharp
Install-Package Creekdream.Cache.Redis
```

### 配置

``` csharp
services.AddRedisCache(
    options =>
    {
        options.ConnectionString = "127.0.0.1";
    });
```

## 三、缓存统一使用示例

#### 1. 简单值存取示例
``` csharp
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
```

#### 2. 对象存取示例
``` csharp
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
```

更多使用示例请参考单元测试。

## 参与贡献
1. Fork Creekdream.AspNetCore 开源框架
2. 新建 feature-\{tag} 分支
3. 完成功能并提交代码
4. 新建 Pull Request

## Change Log

*v0.1.0 2018-09-12*

**Features**
*  支持 Redis 缓存
*  支持 Memory 缓存