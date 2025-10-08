using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Domain.Services;
using API.Domain.Repositories;
using API.Infrastructure.Persistence;
using API.Application.Mappings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace API.Test.Integrations;

public class UserServiceTests : IDisposable
{
    private readonly IUserService _userService;
    private readonly DatabaseContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRoleService _roleService;

    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;

    public UserServiceTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: $"UserServiceTests_{Guid.NewGuid()}")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        _context = new DatabaseContext(options);
        _context.Database.EnsureCreated();

        _userRepository = new UserRepository(_context);
        _roleRepository = new RoleRepository(_context);
        _passwordService = new PasswordService();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<RoleProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _roleService = new RoleService(_roleRepository, _mapper);
        _userService = new UserService(_userRepository, _roleService,_passwordService, _mapper);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
        
        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(user, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _userService.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("John Doe", result.FullName);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _userService.GetUserByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
        
        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(user,TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _userService.GetUserByEmailAsync(user.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("John Doe", result.FullName);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var nonExistentEmail = "nonexistent@email.com";

        // Act
        var result = await _userService.GetUserByEmailAsync(nonExistentEmail);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
        
        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(user, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _userService.GetUserByUsernameAsync(user.Username);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("John Doe", result.FullName);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByUsernameOrEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);

        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );

        await _context.Users.AddAsync(user, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var resultName = await _userService.GetUserByEmailOrUsernameAsync(user.Username);
        var resultEmail = await _userService.GetUserByEmailOrUsernameAsync(user.Email);

        // Assert
        Assert.NotNull(resultName);
        Assert.Equal(user.Id, resultName.Id);
        Assert.Equal("John Doe", resultName.FullName);
        Assert.Equal(user.Username, resultName.Username);
        Assert.Equal(user.Email, resultName.Email);
        
        Assert.NotNull(resultEmail);
        Assert.Equal(user.Id, resultEmail.Id);
        Assert.Equal("John Doe", resultEmail.FullName);
        Assert.Equal(user.Username, resultEmail.Username);
        Assert.Equal(user.Email, resultEmail.Email);
    }

    [Fact]
    public async Task GetUserByUsernameOrEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);

        var nonExistentUsername = "johnd";
        var nonExistentEmail = "johnd@email.com";

        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(user, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var resultName = await _userService.GetUserByEmailOrUsernameAsync(nonExistentUsername);
        var resultEmail = await _userService.GetUserByEmailOrUsernameAsync(nonExistentEmail);

        // Assert
        Assert.Null(resultName);
        Assert.Null(resultEmail);

    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var nonExistentUsername = "nonexistentuser";

        // Act
        var result = await _userService.GetUserByUsernameAsync(nonExistentUsername);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateAndReturnUser_WhenSuccess()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var dto = new CreateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john.doe@example.com",
            Password = "123456",
        };

        // Act
        var result = await _userService.CreateUserAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal("John Doe", result.FullName);
        Assert.NotEqual(Guid.Empty, result.Id);
        
         // Assert that the user is saved in the database
        var dbUser = await _userRepository.GetUserByEmailAsync(dto.Email);
        Assert.NotNull(dbUser);
        Assert.Equal(dto.Username, dbUser.Username);
        Assert.NotNull(dbUser.Password);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        // Arrange
        const string existingEmail = "jane@example.com";
        
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
            
        var existingUser = new User(
            username: "janedoe",
            email: existingEmail,
            firstName: "Jane",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(existingUser, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var dto = new CreateUserDTO
        {
            Username = "johndoe",
            FirstName = "John",
            LastName = "Doe",
            Email = existingEmail,
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.CreateUserAsync(dto));

        Assert.Equal("User with this email already exists.", ex.Message);
    }

    [Fact]
   public async Task CreateUserAsync_ShouldThrow_WhenUsernameAlreadyExists()
   {
         // Arrange
         const string existingUsername = "johndoe";

        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
            
        var existingUser = new User(
            username: existingUsername,
            email: "jane@email.com",
            firstName: "Jane",
            lastName: "Doe",
            role: role,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(existingUser, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var dto = new CreateUserDTO
        {
            Username = existingUsername,
            FirstName = "John",
            LastName = "Doe",
            Email = "another@email.com",
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.CreateUserAsync(dto));

        Assert.Equal("User with this username already exists.", ex.Message);
   }
   
    [Fact]
    public async Task CreateUserAsync_ShouldThrow_WhenDefaultRoleNotFound()
    {
        // Arrange
        var dto = new CreateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john.doe@example.com",
            Password = "123456",
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.CreateUserAsync(dto));

        Assert.Equal("Default role not found.", ex.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldHashPassword()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "User" };
        await _context.Roles.AddAsync(role, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var dto = new CreateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Username = "johndoe",
            Email = "john.doe@example.com",
            Password = "123456",
        };

        // Act
        await _userService.CreateUserAsync(dto);
        var dbUser = await _userRepository.GetUserByEmailAsync(dto.Email);

        // Assert
        Assert.NotNull(dbUser);
        Assert.NotEqual(dto.Password, dbUser.Password);
        Assert.NotNull(dbUser.Password);
    }

    [Fact]
    public async Task UpgradeUserRoleAsync_ShouldUpgradeRole_WhenUserAndRoleExist()
    {
        // Arrange
        var userRole = new Role { Id = 1, Name = "User" };
        var premiumRole = new Role { Id = 2, Name = "Premium" };
        
        await _context.Roles.AddRangeAsync(userRole, premiumRole);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: userRole,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(user, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _userService.GetUserByIdAsync(user.Id);
        
        await _userService.UpgradeUserRoleAsync(user.Id, premiumRole.Id);
        var upgradedUser = await _userService.GetUserByIdAsync(user.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.NotNull(upgradedUser);
        Assert.Equal(premiumRole.Name, upgradedUser.RoleName);
    }
    
    [Fact]
    public async Task UpgradeUserRoleAsync_ShouldThrowException_WhenUserAlreadyHasTheRole()
    {
        // Arrange
        var userRole = new Role { Id = 1, Name = "User" };

        await _context.Roles.AddRangeAsync(userRole);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var user = new User(
            username: "johndoe",
            email: "johndoe@email.com",
            firstName: "John",
            lastName: "Doe",
            role: userRole,
            passwordHash: "hashedpassword"
        );
        
        await _context.Users.AddAsync(user, TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
           _userService.UpgradeUserRoleAsync(user.Id, userRole.Id));

        Assert.Equal("User already has this role.", ex.Message);
    }   
}