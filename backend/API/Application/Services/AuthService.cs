using API.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using API.Application.Interfaces;

using AutoMapper;

namespace API.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ICacheService _cacheService;
    private readonly IPasswordService _passwordService;

    public AuthService
    (
        ICacheService cacheService,
        IUserRepository userRepository,
        IMapper mapper,
        ITokenService tokenService,
        IUserService userService,
        IPasswordService passwordService
    )
    {
        _cacheService = cacheService;
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _userService = userService;
        _passwordService = passwordService;
    }

    public async Task<ApiResponse<UserDTO>> AuthenticateAsync(LoginRequestDto loginDto)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailOrUsernameAsync(loginDto.Email);

            if (user == null || !_passwordService.VerifyPassword(user.Password, loginDto.Password))
            {
                return new ApiResponse<UserDTO>
                {
                    IsSuccess = false,
                    Message = "Email/Username or password is incorrect.",
                    Data = null
                };
            }

            var userDto = _mapper.Map<UserDTO>(user);
            var tokenString = _tokenService.GenerateToken(userDto);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _cacheService.CacheRefreshToken(userDto.Username, refreshToken);

            userDto.Token = tokenString;
            userDto.RefreshToken = refreshToken;

            return new ApiResponse<UserDTO>
            {
                IsSuccess = true,
                Message = "Login successful.",
                Data = userDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserDTO>
            {
                IsSuccess = false,
                Message = $"Authenticate error: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<UserDTO>> RegisterUserAsync(CreateUserDTO createUserDto)
    {
        try
        {
            var userDto = await _userService.CreateUserAsync(createUserDto);
            if (userDto == null)
            {
                return new ApiResponse<UserDTO>
                {
                    IsSuccess = false,
                    Message = "Invalid user data or user already exists.",
                    Data = null
                };
            }

            var token = _tokenService.GenerateToken(userDto.Data);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _cacheService.CacheRefreshToken(userDto.Username, refreshToken);

            userDto.Token = token;
            userDto.RefreshToken = refreshToken;

            return new ApiResponse<UserDTO>
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                Data = userDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserDTO>
            {
                IsSuccess = false,
                Message = $"Register error: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<ApiResponse<UserDTO>> RefreshTokenAsync(string refreshToken)
    {
        var userId = _tokenService.ValidateRefreshToken(refreshToken);
        if (string.IsNullOrEmpty(userId))
            return new ApiResponse<UserDTO>
            {
                IsSuccess = false,
                Message = "Invalid refresh token",
                Data = null
            };

        var cachedToken = await _cacheService.GetRefreshToken(userId);
        if (cachedToken != refreshToken)
            return new ApiResponse<UserDTO>
            {
                IsSuccess = false,
                Message = "Refresh token mismatch",
                Data = null
            };

        var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
        if (user == null)
            return new ApiResponse<UserDTO>
            {
                IsSuccess = false,
                Message = "User not found",
                Data = null
            };

        var userDto = _mapper.Map<UserDTO>(user);
        var newAccessToken = _tokenService.GenerateToken(userDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        await _cacheService.CacheRefreshToken(userDto.Id.ToString(), newRefreshToken);

        userDto.Token = newAccessToken;
        userDto.RefreshToken = newRefreshToken;

        return new ApiResponse<UserDTO>
        {
            IsSuccess = true,
            Message = "Token refreshed successfully",
            Data = userDto
        };
    }

    public async Task<ApiResponse<object>> RevokeRefreshTokenAsync(string token)
    {

        var username = _tokenService.GetUsernameFromToken(token);
        if (string.IsNullOrEmpty(username))
        {
            return new ApiResponse<object>
            {
                IsSuccess = false,
                Message = "Invalid token",
                Data = null
            };
        }
        
        await _cacheService.DeleteRefreshToken(username);

        return new ApiResponse<object>
        {
            IsSuccess = true,
            Message = "Token revoked successfully",
            Data = new { }
        };
    }
}
