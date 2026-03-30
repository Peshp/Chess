namespace Chess.Web.Infrastructure.Extension;

using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

public static class RedisExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, 
        string key, 
        T data, 
        TimeSpan absoluteExpireTime)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(key, jsonData, options);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string key)
    {
        var jsonData = await cache.GetStringAsync(key);
        return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
    }
}
