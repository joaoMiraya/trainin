using API.Application.Interfaces;

namespace API.Application.Services;


public class CacheService : ICacheService
{
    private readonly ICacheRepository _cache;

    public CacheService(ICacheRepository cache)
    {
        _cache = cache;
    }

    public async Task CacheUserSessionAsync(string username, object sessionData)
    {
        await _cache.SetAsync($"session:{username}", sessionData, TimeSpan.FromMinutes(30));
    }

    public async Task<object?> GetUserSessionAsync(string username)
    {
        return await _cache.GetAsync<object>($"session:{username}");
    }

    public async Task CacheRefreshToken(string username, string token)
    {
        await _cache.SetAsync($"refreshToken:{username}", token, TimeSpan.FromDays(7));
    }

    public async Task<string?> GetRefreshToken(string username)
    {
        return await _cache.GetAsync<string>($"refreshToken:{username}");
    }
    public async Task DeleteRefreshToken(string username)
    {
        await _cache.RemoveAsync($"refreshToken:{username}");
    }
}
