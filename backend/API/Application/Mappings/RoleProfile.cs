

using API.Application.DTOs;
using API.Domain.Entities;
using AutoMapper;

namespace API.Application.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        // Entity -> DTO
        CreateMap<Role, RoleDTO>();
        CreateMap<Role, RoleWithUsersDTO>();

        // DTO -> Entity
        CreateMap<RoleDTO, Role>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<RoleWithUsersDTO, Role>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Map users dentro do RoleWithUsersDTO
        CreateMap<User, BasicUserDTO>();
    }
}
