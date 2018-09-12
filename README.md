# Creekdream �������

����Ŀ���������������.NET STANDARD 2.0 ����Ŀ��Ŀǰ֧�� Redis �����Լ� Memory ���档

## һ��Creekdream Memory ����

�ڴ滺�档

### ��װ

``` csharp
Install-Package Creekdream.Cache.Memory
```

### ����

``` csharp
services.AddMemoryCache(options => { });
```

## ����Creekdream Redis ����

Redis ������ʹ�ù����У��뾡����Ҫ�洢���ݹ��󣬿��ܻ���ɳ�ʱ�����⡣

### ��װ

``` csharp
Install-Package Creekdream.Cache.Redis
```

### ����

``` csharp
services.AddRedisCache(
    options =>
    {
        options.ConnectionString = "127.0.0.1";
    });
```

## ��������ͳһʹ��ʾ��

#### 1. ��ֵ��ȡʾ��
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

#### 2. �����ȡʾ��
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

����ʹ��ʾ����ο���Ԫ���ԡ�

## ���빱��
1. Fork Creekdream.AspNetCore ��Դ���
2. �½� feature-\{tag} ��֧
3. ��ɹ��ܲ��ύ����
4. �½� Pull Request

## Change Log

*v0.1.0 2018-09-12*

**Features**
*  ֧�� Redis ����
*  ֧�� Memory ����