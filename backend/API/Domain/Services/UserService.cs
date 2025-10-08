using System.ComponentModel.DataAnnotations;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Shared.Constants;
using AutoMapper;

namespace API.Domain.Services;

public class UserService : IUserService
{
    private readonly INotificationContext _notificationContext;
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;
    private readonly IPasswordService _passwordService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public UserService(
        INotificationContext notificationContext,
        IUserRepository userRepository,
        IRoleService roleService,
        IPasswordService passwordService,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _notificationContext = notificationContext;
        _userRepository = userRepository;
        _roleService = roleService;
        _passwordService = passwordService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var existingEmail = await _userRepository.GetUserByEmailAsync(createUserDto.Email);
            if (existingEmail is not null)
                _notificationContext.AddError("User with this email already exists.",
                    nameof(createUserDto.Email), NotificationSystemType.Error.ToString());

            var existingUsername = await _userRepository.GetUserByUsernameAsync(createUserDto.Username);
            if (existingUsername is not null)
                _notificationContext.AddError("User with this username already exists.",
                    nameof(createUserDto.Username), NotificationSystemType.Error.ToString());

            var role = await _roleService.GetDefaultRoleAsync();
            if (role == null)
                _notificationContext.AddError("Default role not found.",
                    nameof(role), NotificationSystemType.Error.ToString());

            var hashedPassword = _passwordService.HashPassword(createUserDto.Password);
            var user = _mapper.Map<User>(createUserDto);
            user.Password = hashedPassword;
            user.Role = _mapper.Map<Role>(role);

            await _userRepository.CreateAsync(user);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _notificationContext.AddError("An error occurred while creating the user: ",
                ex.Message, NotificationSystemType.Error.ToString());
            throw;
        }
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _notificationContext.AddWarning("The provided ID is empty.",
                nameof(id), NotificationSystemType.Warning.ToString());
            return null;
        }
        
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _notificationContext.AddWarning("User not found.",
                nameof(id), NotificationSystemType.Warning.ToString());
            return null;
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _notificationContext.AddWarning("The provided email is null or empty.",
                nameof(email), NotificationSystemType.Warning.ToString());
            return null;
        }

        if (!new EmailAddressAttribute().IsValid(email))
        {
            _notificationContext.AddWarning("The provided email is not valid.",
                nameof(email), NotificationSystemType.Warning.ToString());
            return null;
        }

        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO?> GetUserByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            _notificationContext.AddWarning("The provided username is null or empty.",
                nameof(username), NotificationSystemType.Warning.ToString());
            return null;
        }
        
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null)
        {
            return null;
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO?> GetUserByEmailOrUsernameAsync(string emailOrUsername)
    {
        if (string.IsNullOrWhiteSpace(emailOrUsername))
        {
            _notificationContext.AddWarning("The provided email or username is null or empty.",
                nameof(emailOrUsername), NotificationSystemType.Warning.ToString());
            return null;
        }

        var user = await _userRepository.GetUserByEmailOrUsernameAsync(emailOrUsername);
        if (user == null)
        {
            return null;
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<bool> UpgradeUserRoleAsync(Guid userId, int roleId)
    {
        if (userId == Guid.Empty)
        {
            _notificationContext.AddWarning("The provided ID is empty.",
                nameof(userId), NotificationSystemType.Info.ToString());
            return false;
        }
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            _notificationContext.AddWarning("User not found.",
                nameof(userId), NotificationSystemType.Info.ToString());
            return false;
        }

        var role = await _roleService.GetRoleByIdAsync(roleId);
        if (role is null)
        {
            _notificationContext.AddWarning("Role not found.",
                nameof(roleId), NotificationSystemType.Info.ToString());
            return false;
        }

        if (user.RoleId == role.Id)
        {
            _notificationContext.AddInfo("User already has this role.",
                nameof(roleId), NotificationSystemType.Info.ToString());
            return false;
        }

        var mappedRole = _mapper.Map<Role>(role);
        await _userRepository.UpgradeUserRoleAsync(user, mappedRole);

        return true;
    }
}
