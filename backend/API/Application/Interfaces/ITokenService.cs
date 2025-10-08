
using API.Application.DTOs;

namespace API.Application.Interfaces;
public interface ITokenService
{
    string GenerateRefreshToken();
    string GenerateToken(UserDTO userDto);
    string? GetUsernameFromToken(string token);
    string? ValidateRefreshToken(string token);
}