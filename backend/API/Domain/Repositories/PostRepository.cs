

using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Repositories;

public class PostRepository : IPostRepository
{
    private readonly DatabaseContext _context;

    public PostRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreatePostAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}