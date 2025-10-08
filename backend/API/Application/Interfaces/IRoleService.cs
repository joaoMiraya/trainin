using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IRoleService
{
    Task<RoleDTO?> GetDefaultRoleAsync();
    Task<RoleDTO?> GetRoleByIdAsync(int id);
    Task<RoleDTO?> GetRoleByNameAsync(string name);
}
