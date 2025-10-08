

using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface IPostRepository
{
    Task CreatePostAsync(Post post);
    Task<Post?> GetPostByIdAsync(int id);
    Task SaveChangesAsync();
}