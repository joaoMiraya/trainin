using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IPostService
{
    Task<PostDto?> CreatePostAsync(CreatePostDto dto);
    Task<PostDto?> GetPostByIdAsync(int id);
}
