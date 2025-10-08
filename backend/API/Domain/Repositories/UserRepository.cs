using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Role) // Include the Role navigation property - Eager Loading 
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByEmailOrUsernameAsync(string emailOrUsername)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == emailOrUsername || u.Username == emailOrUsername);
    }

    public Task UpgradeUserRoleAsync(User user, Role role)
    {
        if (user.RoleId == role.Id)
            return Task.CompletedTask;

        user.Role = role;
        user.RoleId = role.Id;
        _context.Users.Update(user);
        return _context.SaveChangesAsync();
    }
}