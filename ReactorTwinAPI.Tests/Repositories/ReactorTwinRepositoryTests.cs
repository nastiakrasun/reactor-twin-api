using Xunit;
using Microsoft.EntityFrameworkCore;
using ReactorTwinAPI.Features.ReactorTwins.Repositories;
using ReactorTwinAPI.Features.ReactorTwins.Dtos;
using ReactorTwinAPI.Infrastructure.Persistence;
using ReactorTwinAPI.Domain.Entities;
using AutoMapper;

namespace ReactorTwinAPI.Tests.Repositories;

public class ReactorTwinRepositoryTests : IDisposable
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ReactorTwinRepository _repository;

    public ReactorTwinRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ReactorTwinAPI.Features.ReactorTwins.Mapping.ReactorTwinProfile>();
            cfg.AddProfile<ReactorTwinAPI.Features.Users.Mapping.UserProfile>();
        });
        _mapper = config.CreateMapper();

        _repository = new ReactorTwinRepository(_dbContext, _mapper);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact]
    public async Task CreateAsync_CreatesReactorTwin()
    {
        // Arrange
        var createDto = new CreateReactorTwinDto { Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };
        var ownerId = Guid.NewGuid();
        var owner = new User { Id = ownerId, Username = "testuser", PasswordHash = "hash" };
        _dbContext.Users.Add(owner);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.CreateAsync(createDto, ownerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Reactor", result.Name);
        Assert.Equal("Model1", result.Model);
        Assert.Equal(ownerId, result.OwnerId);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotNull(result.Owner);
        Assert.Equal(ownerId, result.Owner.Id);
        Assert.Equal("testuser", result.Owner.Username);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsReactor()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var owner = new User { Id = ownerId, Username = "testuser", PasswordHash = "hash" };
        _dbContext.Users.Add(owner);
        var reactor = new ReactorTwin { Id = Guid.NewGuid(), Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1", OwnerId = ownerId };
        _dbContext.ReactorTwins.Add(reactor);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(reactor.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reactor.Id, result.Id);
        Assert.Equal("Test Reactor", result.Name);
        Assert.NotNull(result.Owner);
        Assert.Equal(ownerId, result.Owner.Id);
        Assert.Equal("testuser", result.Owner.Username);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllReactors()
    {
        // Arrange
        var ownerId1 = Guid.NewGuid();
        var owner1 = new User { Id = ownerId1, Username = "user1", PasswordHash = "hash1" };
        var ownerId2 = Guid.NewGuid();
        var owner2 = new User { Id = ownerId2, Username = "user2", PasswordHash = "hash2" };
        _dbContext.Users.AddRange(owner1, owner2);
        var reactor1 = new ReactorTwin { Id = Guid.NewGuid(), Name = "Reactor 1", Model = "Model1", SerialNumber = "SN1", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1", OwnerId = ownerId1 };
        var reactor2 = new ReactorTwin { Id = Guid.NewGuid(), Name = "Reactor 2", Model = "Model2", SerialNumber = "SN2", ReactorType = "Type2", FuelType = "Fuel2", CoolingSystemType = "Cooling2", OwnerId = ownerId2 };
        _dbContext.ReactorTwins.AddRange(reactor1, reactor2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, r => r.Name == "Reactor 1");
        Assert.Contains(list, r => r.Name == "Reactor 2");
        foreach (var r in list)
        {
            Assert.NotNull(r.Owner);
        }
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesReactor()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var owner = new User { Id = ownerId, Username = "testuser", PasswordHash = "hash" };
        _dbContext.Users.Add(owner);
        var reactor = new ReactorTwin { Id = Guid.NewGuid(), Name = "Old Name", Model = "Old Model", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1", OwnerId = ownerId };
        _dbContext.ReactorTwins.Add(reactor);
        await _dbContext.SaveChangesAsync();

        var updateDto = new UpdateReactorTwinDto { Name = "New Name", Model = "New Model", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1" };

        // Act
        var result = await _repository.UpdateAsync(reactor.Id, updateDto);

        // Assert
        Assert.True(result);
        var updated = await _dbContext.ReactorTwins.Include(r => r.Owner).FirstOrDefaultAsync(r => r.Id == reactor.Id);
        Assert.NotNull(updated);
        Assert.Equal("New Name", updated.Name);
        Assert.Equal("New Model", updated.Model);
        Assert.NotNull(updated.Owner);
        Assert.Equal(ownerId, updated.Owner.Id);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsFalse()
    {
        // Arrange
        var updateDto = new UpdateReactorTwinDto { Name = "New Name" };

        // Act
        var result = await _repository.UpdateAsync(Guid.NewGuid(), updateDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesReactor()
    {
        // Arrange
        var reactor = new ReactorTwin { Id = Guid.NewGuid(), Name = "Test Reactor", Model = "Model1", SerialNumber = "SN123", ReactorType = "Type1", FuelType = "Fuel1", CoolingSystemType = "Cooling1", OwnerId = Guid.NewGuid() };
        _dbContext.ReactorTwins.Add(reactor);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(reactor.Id);

        // Assert
        Assert.True(result);
        var deleted = await _dbContext.ReactorTwins.FindAsync(reactor.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _repository.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}