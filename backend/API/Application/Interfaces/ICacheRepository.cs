
namespace API.Application.Interfaces;

public interface ICacheRepository
{
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
}