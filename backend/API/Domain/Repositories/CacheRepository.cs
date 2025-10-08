using API.Application.Interfaces;
using API.Infrastructure.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace API.Domain.Repositories;

public class CacheRepository : ICacheRepository
{
    private readonly IDatabase _db;

    public CacheRepository(RedisContext context)
    {
        _db = context.Database;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue
            ? JsonSerializer.Deserialize<T>(value!)
            : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
}
