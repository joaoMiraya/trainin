
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IUserService
{
    Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto, CancellationToken cancellationToken = default);
    Task<UserDTO?> GetUserByEmailAsync(string email);
    Task<UserDTO?> GetUserByEmailOrUsernameAsync(string emailOrUsername);
    Task<UserDTO?> GetUserByIdAsync(Guid id);
    Task<UserDTO?> GetUserByUsernameAsync(string username);
    Task<bool> UpgradeUserRoleAsync(Guid userId, int roleId);
}