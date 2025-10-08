
namespace API.Application.Interfaces;

public interface ICacheService
{
    Task CacheRefreshToken(string username, string token);
    Task CacheUserSessionAsync(string username, object sessionData);
    Task DeleteRefreshToken(string username);
    Task<string?> GetRefreshToken(string username);
    Task<object?> GetUserSessionAsync(string username);
}
