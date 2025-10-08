

using API.Application.Interfaces;

namespace API.Application.Services;

public class SystemService : ISystemService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SystemService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public Guid? GetCurrentUserId()
    {
        if (!IsUserAuthenticated())
        {
            return null;
        }

        var idString = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;

        if (Guid.TryParse(idString, out var userId))
        {
            return userId;
        }

        return null;
    }

}