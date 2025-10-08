using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDTO
{
    [Required]
    [MaxLength(50)]
    [MinLength(3, ErrorMessage = "The username must be at least 3 characters long")]
    [DataType(DataType.Text)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [MinLength(3, ErrorMessage = "The username must be at least 3 characters long")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

public class BasicUserDTO
{ 
    public string Username { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}