
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<UserDTO>> AuthenticateAsync(LoginRequestDto loginDto);
    Task<ApiResponse<UserDTO>> RefreshTokenAsync(string refreshToken);
    Task<ApiResponse<UserDTO>> RegisterUserAsync(CreateUserDTO createUserDto);
    Task<ApiResponse<object>> RevokeRefreshTokenAsync(string refreshToken);
}