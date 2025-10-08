

using API.Application.DTOs;
using API.Application.Interfaces;
using AutoMapper;

namespace API.Domain.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<RoleDTO?> GetRoleByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Role ID must be greater than zero.", nameof(id));

        var role = await _roleRepository.GetRoleByIdAsync(id);
        return role is null ? null : _mapper.Map<RoleDTO>(role);
    }

    public async Task<RoleDTO?> GetRoleByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be null or empty.", nameof(name));

        var role = await _roleRepository.GetRoleByNameAsync(name);
        return role is null ? null : _mapper.Map<RoleDTO>(role);
    }

    public async Task<RoleDTO?> GetDefaultRoleAsync()
    {
        var role = await _roleRepository.GetDefaultRoleAsync();

        return role is null ? null : _mapper.Map<RoleDTO>(role);
    }
}