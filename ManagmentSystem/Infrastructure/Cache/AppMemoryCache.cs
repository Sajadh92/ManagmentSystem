using Infrastructure.Service;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

public interface IAppMemoryCache
{
    TItem Get<TItem>(string key);
    void Set(string key, object value);
    void Set(string key, object value, int ExpireInSeconds);
    bool IsExist(string key);
    void Remove(string key);
}

public class AppMemoryCache : IAppMemoryCache, ISingleton
{
    private readonly IMemoryCache _cache;

    public AppMemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public TItem Get<TItem>(string key)
    {
        try
        {
            _cache.TryGetValue(key, out TItem value);

            return value;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Set(string key, object value)
    {
        try
        {
            _cache.Set(key, value);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Set(string key, object value, int ExpireInSeconds)
    {
        try
        {
            _cache.Set(key, value, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(ExpireInSeconds)));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool IsExist(string key)
    {
        try
        {
            return _cache.TryGetValue<object>(key, out object value);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Remove(string key)
    {
        try
        {
            _cache.Remove(key);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
