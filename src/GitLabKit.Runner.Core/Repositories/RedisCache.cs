using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;

namespace GitLabKit.Runner.Core.Repositories;

public class RedisCache : ICache
{
    private readonly IConnectionMultiplexer _redis;

    public RedisCache(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public async Task<T> GetCached<T>(string key, Func<Task<T>> dataFunc, TimeSpan ttl)
    {
        try
        {
            var db = _redis.GetDatabase();
            var data = await db.StringGetAsync(key);

            if (data.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(data.ToString());
            }

            var newData = await dataFunc.Invoke();
            await db.StringSetAsync(key, JsonConvert.SerializeObject(newData), ttl);

            return newData;
        }
        catch (Exception e)
        {
            Log.Error(e, "RedisCacheService.GetCached failed");
            return await dataFunc.Invoke();
        }
    }

    public async Task Delete(string key)
    {
        try
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
        catch (Exception e)
        {
            Log.Error(e, "RedisCacheService.Delete failed");
            throw;
        }
    }
}