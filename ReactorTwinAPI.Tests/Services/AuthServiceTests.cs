using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using ReactorTwinAPI.Features.Auth.Services;
using ReactorTwinAPI.Features.Auth.Dtos;
using ReactorTwinAPI.Features.Users.Dtos;
using ReactorTwinAPI.Features.Users.Repositories;
using ReactorTwinAPI.Domain.Entities;

namespace ReactorTwinAPI.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _configMock = new Mock<IConfiguration>();
        // Setup JWT configuration
        _configMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkeythatislongenoughforjwt256bits");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("testissuer");
        _configMock.Setup(c => c["Jwt:Audience"]).Returns("testaudience");
        _authService = new AuthService(_userRepoMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ValidRequest_CreatesUser()
    {
        // Arrange
        var registerDto = new RegisterDto { Username = "testuser", Password = "password", RequestSuper = false };
        var createUserDto = new CreateUserDto { Username = "testuser", IsSuperUser = false };
        var userDto = new UserDto { Id = Guid.NewGuid(), Username = "testuser", IsSuperUser = false, CanCreateReactor = false };

        _userRepoMock.Setup(r => r.AnyUsersAsync()).ReturnsAsync(true);
        _userRepoMock.Setup(r => r.GetByUsernameAsync("testuser")).ReturnsAsync((UserDto?)null);
        _userRepoMock.Setup(r => r.CreateAsync(It.IsAny<CreateUserDto>(), It.IsAny<string>())).ReturnsAsync(userDto);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.Equal(userDto, result);
    }

    [Fact]
    public async Task RegisterAsync_UsernameExists_ThrowsException()
    {
        // Arrange
        var registerDto = new RegisterDto { Username = "existinguser", Password = "password", RequestSuper = false };
        var existingUser = new UserDto { Id = Guid.NewGuid(), Username = "existinguser" };

        _userRepoMock.Setup(r => r.AnyUsersAsync()).ReturnsAsync(true);
        _userRepoMock.Setup(r => r.GetByUsernameAsync("existinguser")).ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(registerDto));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        var userEntity = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), IsSuperUser = false, CanCreateReactor = false };

        _userRepoMock.Setup(r => r.GetEntityByUsernameAsync("testuser")).ReturnsAsync(userEntity);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
        var userEntity = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), IsSuperUser = false, CanCreateReactor = false };

        _userRepoMock.Setup(r => r.GetEntityByUsernameAsync("testuser")).ReturnsAsync(userEntity);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password" };

        _userRepoMock.Setup(r => r.GetEntityByUsernameAsync("nonexistent")).ReturnsAsync((User?)null);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }
}