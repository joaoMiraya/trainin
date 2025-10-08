using AutoMapper;
using API.Domain.Entities;
using API.Application.DTOs;

namespace API.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity -> DTO
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        // DTO -> Entity (para criação de usuários)
        CreateMap<CreateUserDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // DTO -> Simple DTO 
        CreateMap<UserDTO, BasicUserDTO>();
    }
}
