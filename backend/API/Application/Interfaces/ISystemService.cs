
namespace API.Application.Interfaces;

public interface ISystemService
{
    bool IsUserAuthenticated();
    Guid? GetCurrentUserId();
}