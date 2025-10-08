using API.Application.DTOs;
using API.Domain.Entities;
using AutoMapper;

namespace API.Application.Mappings;

public class PostProfile : Profile
{
    public PostProfile()
    {
        // Post => PostDto
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.Url)))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count));

        // CreatePostDto => Post
        CreateMap<CreatePostDto, Post>();

        // UpdatePostDto => Post
        CreateMap<UpdatePostDto, Post>()
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // evita sobrescrever imagens
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // não vem no update
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // não deve mudar
            .ForMember(dest => dest.User, opt => opt.Ignore()); // mantém a navegação atual
    }
}
