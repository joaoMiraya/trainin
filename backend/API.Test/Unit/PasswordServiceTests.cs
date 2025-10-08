
namespace API.Test.Unit;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService;

    public PasswordServiceTests()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hashedPassword = _passwordService.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
    {
        // Arrange
        var password = "TestPassword123";
        var hashedPassword = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(hashedPassword, password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        // Arrange
        var password = "TestPassword123";
        var hashedPassword = _passwordService.HashPassword(password);
        var wrongPassword = "WrongPassword456";

        // Act
        var result = _passwordService.VerifyPassword(hashedPassword, wrongPassword);

        // Assert
        Assert.False(result);
    }
}