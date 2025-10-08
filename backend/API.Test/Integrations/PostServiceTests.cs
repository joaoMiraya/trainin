


using System.Security.Claims;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Application.Mappings;
using API.Application.Services;
using API.Domain.Repositories;
using API.Domain.Services;
using API.Infrastructure.Persistence;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace API.Test.Integrations;

public class PostServiceTests : IDisposable
{
    private readonly IPostService _postService;
    private readonly DatabaseContext _context;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly SystemService _systemService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostServiceTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: $"PostServiceTests_{Guid.NewGuid()}")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        _context = new DatabaseContext(options);
        _context.Database.EnsureCreated();

        _postRepository = new PostRepository(_context);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PostProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };

        _systemService = new SystemService(_httpContextAccessor);
        _postService = new PostService(_postRepository, _mapper, _systemService);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task CreatePostAsync_ShouldCreatePost_WhenUserIsAuthenticated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new Claim("id", userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var userPrincipal = new ClaimsPrincipal(identity);

        _httpContextAccessor.HttpContext.User = userPrincipal;

        var createDto = new CreatePostDto
        {
            Title = "Integration Test Title",
            Content = "This is content for integration test.",
            ImageUrls = new List<string> { "http://fakeimage.com/imge1.png" }
        };

        // Act
        var result = await _postService.CreatePostAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be(createDto.Title);
        result.Content.Should().Be(createDto.Content);
        result.UserId.Should().Be(userId);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task CreatePostAsync_ShouldThrowUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal();

        var dto = new CreatePostDto
        {
            Title = "No Auth",
            Content = "Should fail"
        };

        var act = async () => await _postService.CreatePostAsync(dto);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("User is not authenticated.");
    }
    
    [Fact]
    public async Task GetPostByIdAsync_ShouldReturnPost_WhenPostExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new Claim("id", userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var userPrincipal = new ClaimsPrincipal(identity);

        _httpContextAccessor.HttpContext.User = userPrincipal;

        var dto = new CreatePostDto
        {
            Title = "GetPost Test",
            Content = "Content for retrieval"
        };

        var created = await _postService.CreatePostAsync(dto);
        var fetched = await _postService.GetPostByIdAsync(created!.Id);

        fetched.Should().NotBeNull();
        fetched!.Id.Should().Be(created.Id);
    }
}   