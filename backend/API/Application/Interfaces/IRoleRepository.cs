using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByIdAsync(int id);
    Task<Role?> GetRoleByNameAsync(string name);
    Task<Role?> GetDefaultRoleAsync();
}