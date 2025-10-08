

using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using AutoMapper;
using API.Application.Services;

namespace API.Domain.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly SystemService _systemService;

    public PostService(IPostRepository postRepository, IMapper mapper, SystemService systemService)
    {
        _postRepository = postRepository;
        _mapper = mapper;
        _systemService = systemService;
    }

    public async Task<PostDto?> CreatePostAsync(CreatePostDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto), "Post DTO cannot be null.");

        var userId = _systemService.GetCurrentUserId();
        if (userId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        var post = new Post(dto.Title, dto.Content, userId.Value);

        await _postRepository.CreatePostAsync(post);
        await _postRepository.SaveChangesAsync();

        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        var post = await _postRepository.GetPostByIdAsync(id);
        return post is null ? null : _mapper.Map<PostDto>(post);
    }
}