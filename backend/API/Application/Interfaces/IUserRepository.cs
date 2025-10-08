using API.Domain.Entities;
using API.Domain.Repositories;

namespace API.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
    Task UpgradeUserRoleAsync(User user, Role role);
    Task<User?> GetUserByEmailOrUsernameAsync(string emailOrUsername);
}