
namespace API.Application.DTOs;

public class LoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; }
}

