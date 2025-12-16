using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ReactorTwinAPI.Features.Auth.Controllers;
using ReactorTwinAPI.Features.Auth.Services;
using ReactorTwinAPI.Features.Auth.Dtos;
using ReactorTwinAPI.Features.Users.Dtos;

namespace ReactorTwinAPI.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_ValidRequest_ReturnsOk()
    {
        // Arrange
        var registerDto = new RegisterDto { Username = "testuser", Password = "password", RequestSuper = false };
        var userDto = new UserDto { Id = Guid.NewGuid(), Username = "testuser", IsSuperUser = false, CanCreateReactor = false };
        _authServiceMock.Setup(s => s.RegisterAsync(registerDto)).ReturnsAsync(userDto);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(userDto, okResult.Value);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        var token = "jwt-token";
        _authServiceMock.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(token);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
        _authServiceMock.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync((string?)null);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }
}