

using API.Application.Services;
using Microsoft.AspNetCore.Http;

namespace API.Test.Unit;

public class SystemServiceTests
{
    private readonly SystemService _systemService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SystemServiceTests()
    {
        _httpContextAccessor = new HttpContextAccessor();
        _systemService = new SystemService(_httpContextAccessor);
    }

    [Fact]
    public void IsUserAuthenticated_ShouldReturnFalse_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();

        // Act
        var result = _systemService.IsUserAuthenticated();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetCurrentUserId_ShouldReturnNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext = new DefaultHttpContext();

        // Act
        var result = _systemService.GetCurrentUserId();

        // Assert
        Assert.Null(result);
    }

}