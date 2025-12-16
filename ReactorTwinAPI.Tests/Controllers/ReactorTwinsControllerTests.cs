using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ReactorTwinAPI.Features.ReactorTwins.Controllers;
using ReactorTwinAPI.Features.ReactorTwins.Services;
using ReactorTwinAPI.Features.ReactorTwins.Dtos;

namespace ReactorTwinAPI.Tests.Controllers;

public class ReactorTwinsControllerTests
{
    private readonly Mock<IReactorTwinService> _serviceMock;
    private readonly ReactorTwinsController _controller;

    public ReactorTwinsControllerTests()
    {
        _serviceMock = new Mock<IReactorTwinService>();
        _controller = new ReactorTwinsController(_serviceMock.Object);
    }

    [Fact]
    public async Task Create_ValidDto_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateReactorTwinDto { Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };
        var reactorDto = new ReactorTwinDto { Id = Guid.NewGuid(), Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };
        _serviceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(reactorDto);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(reactorDto, createdResult.Value);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
    }

    [Fact]
    public async Task Create_Unauthorized_ThrowsException_ReturnsForbid()
    {
        // Arrange
        var createDto = new CreateReactorTwinDto { Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };
        _serviceMock.Setup(s => s.CreateAsync(createDto)).ThrowsAsync(new UnauthorizedAccessException());

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reactorDto = new ReactorTwinDto { Id = id, Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };
        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(reactorDto);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(reactorDto, okResult.Value);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ReactorTwinDto?)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Arrange
        var reactors = new List<ReactorTwinDto>
        {
            new ReactorTwinDto { Id = Guid.NewGuid(), Name = "Reactor1", Model = "Model1", SerialNumber = "SN1", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" },
            new ReactorTwinDto { Id = Guid.NewGuid(), Name = "Reactor2", Model = "Model2", SerialNumber = "SN2", ReactorType = "Type2", FuelType = "Fuel2", CoolingSystemType = "Cooling2" }
        };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(reactors);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(reactors, okResult.Value);
    }

    [Fact]
    public async Task Update_ValidDto_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateReactorTwinDto { Name = "Updated Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };
        _serviceMock.Setup(s => s.UpdateAsync(id, updateDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdateReactorTwinDto { Name = "Updated Name" };
        _serviceMock.Setup(s => s.UpdateAsync(id, updateDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.Update(id, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}