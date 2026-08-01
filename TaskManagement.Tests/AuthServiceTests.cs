using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Services;
using TaskManagement.Application.Settings;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        var store = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);

        _jwtSettings = Options.Create(new JwtSettings
        {
            SecretKey = "THIS-IS-A-TEST-SECRET-KEY-32-CHARACTERS-MIN",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        });

        _sut = new AuthService(_mockUserManager.Object, _jwtSettings);
    }


    [Fact]
    public async System.Threading.Tasks.Task LoginUserAsync_WhenUserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        _mockUserManager.Setup(u => u.FindByEmailAsync("notfound@test.com"))
                        .ReturnsAsync((User?)null);
        _mockUserManager.Setup(u => u.FindByNameAsync("notfound@test.com"))
                        .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _sut.LoginUserAsync("notfound@test.com", "somePassword"));
    }
    [Fact]
    public async System.Threading.Tasks.Task LoginUserAsync_WhenUserAreFound_AndPasswordIsCorrect_ReturnsToken()
    {
        // Arrange
        var user = new User { Id = "user123", Email = "Correct@test.com", UserName = "TrueUser" };

        // Mock the FindByEmailAsync and FindByNameAsync methods to return the user
        _mockUserManager.Setup(u => u.FindByEmailAsync("Correct@test.com"))
                        .ReturnsAsync(user);
        _mockUserManager.Setup(u => u.FindByNameAsync("Correct@test.com"))
                        .ReturnsAsync(user);

        // Mock the CheckPasswordAsync method to return true for the correct password
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, "correctPassword"))
                        .ReturnsAsync(true);
        // Act
        var token = await _sut.LoginUserAsync("Correct@test.com", "correctPassword");
        // Assert
        Assert.NotNull(token);

    }

    [Fact]
    public async System.Threading.Tasks.Task LoginUserAsync_WhenPasswordIsIncorrect_ThrowsUnauthorizedException()
    {
        // Arrange
        var user = new User { Id = "user123", Email = "test@test.com", UserName = "testuser" };
        _mockUserManager.Setup(u => u.FindByEmailAsync("test@test.com")).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, "wrongPassword")).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            _sut.LoginUserAsync("test@test.com", "wrongPassword"));
    }


    [Fact]
    public async System.Threading.Tasks.Task RegisterUserAsync_WhenValid_CreatesUserSuccessfully()
    {
        // Arrange
        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), "Password123!"))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        await _sut.RegisterUserAsync("John Doe", "john@test.com", "johndoe", "Password123!");

        // Assert
        _mockUserManager.Verify(u => u.CreateAsync(
            It.Is<User>(usr => usr.Email == "john@test.com" && usr.FullName == "John Doe" && usr.UserName == "johndoe"),
            "Password123!"), Times.Once);
    }


    [Fact]
    public async System.Threading.Tasks.Task RegisterUserAsync_WhenCreationFails_ThrowsBadRequestException()
    {
        // Arrange
        var identityErrors = new List<IdentityError>
        {
            new() { Description = "Email already taken." }
        };
        var failedResult = IdentityResult.Failed(identityErrors.ToArray());

        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(failedResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            _sut.RegisterUserAsync("Jane Doe", "jane@test.com", "janedoe", "weak"));

        Assert.Contains("Email already taken.", exception.Message);
    }

}
