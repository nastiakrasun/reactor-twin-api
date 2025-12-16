using Moq;
using Xunit;
using ReactorTwinAPI.Features.ReactorTwins.Services;
using ReactorTwinAPI.Features.ReactorTwins.Repositories;
using ReactorTwinAPI.Features.ReactorTwins.Dtos;
using ReactorTwinAPI.Application.Services;

namespace ReactorTwinAPI.Tests.Services;

public class ReactorTwinServiceTests
{
    private readonly Mock<IReactorTwinRepository> _repoMock;
    private readonly Mock<ICurrentUserService> _currentUserMock;
    private readonly ReactorTwinService _service;

    public ReactorTwinServiceTests()
    {
        _repoMock = new Mock<IReactorTwinRepository>();
        _currentUserMock = new Mock<ICurrentUserService>();
        _service = new ReactorTwinService(_repoMock.Object, _currentUserMock.Object);
    }

    [Fact]
    public async Task CreateAsync_UserCanCreate_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateReactorTwinDto { Name = "Test Reactor" };
        var userId = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = Guid.NewGuid(), Name = "Test Reactor", OwnerId = userId };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _currentUserMock.Setup(c => c.IsSuperUser).Returns(false);
        _currentUserMock.Setup(c => c.CanCreateReactor).Returns(true);
        _repoMock.Setup(r => r.CreateAsync(createDto, userId)).ReturnsAsync(reactorDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.Equal(reactorDto, result);
    }

    [Fact]
    public async Task CreateAsync_UserNotAuthorized_ThrowsUnauthorized()
    {
        // Arrange
        var createDto = new CreateReactorTwinDto { Name = "Test Reactor" };
        var userId = Guid.NewGuid();

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _currentUserMock.Setup(c => c.IsSuperUser).Returns(false);
        _currentUserMock.Setup(c => c.CanCreateReactor).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsReactor()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = id, Name = "Test Reactor" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(reactorDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Equal(reactorDto, result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllReactors()
    {
        // Arrange
        var reactors = new List<ReactorTwinDto>
        {
            new ReactorTwinDto { Id = Guid.NewGuid(), Name = "Reactor 1" },
            new ReactorTwinDto { Id = Guid.NewGuid(), Name = "Reactor 2" }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reactors);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(reactors, result);
    }

    [Fact]
    public async Task UpdateAsync_OwnerUpdates_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateReactorTwinDto { Name = "Updated Name" };
        var userId = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = id, Name = "Old Name", OwnerId = userId };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _currentUserMock.Setup(c => c.IsSuperUser).Returns(false);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(reactorDto);
        _repoMock.Setup(r => r.UpdateAsync(id, updateDto)).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_NotOwner_ThrowsUnauthorized()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateReactorTwinDto { Name = "Updated Name" };
        var userId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = id, Name = "Old Name", OwnerId = ownerId };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _currentUserMock.Setup(c => c.IsSuperUser).Returns(false);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(reactorDto);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.UpdateAsync(id, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_OwnerDeletes_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = id, Name = "Test Reactor", OwnerId = userId };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _currentUserMock.Setup(c => c.IsSuperUser).Returns(false);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(reactorDto);
        _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NotOwner_ThrowsUnauthorized()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = id, Name = "Test Reactor", OwnerId = ownerId };

        _currentUserMock.Setup(c => c.UserId).Returns(userId);
        _currentUserMock.Setup(c => c.IsSuperUser).Returns(false);
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(reactorDto);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteAsync(id));
    }
}