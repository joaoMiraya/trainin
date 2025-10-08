using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Persistence;
using API.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DatabaseContext _context;

    public RoleRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Role?> GetDefaultRoleAsync()
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == Roles.User);
    }
}