using AutoFixture;
using ShipManagement.Domain.Entities;
using ShipManagement.Infrastructure.Persistence;

namespace ShipManagement.Infrastructure.Tests.Persistence;

public class ShipRepositoryTests
{
    private readonly IFixture _fixture;
    private readonly ShipRepository _sut;

    public ShipRepositoryTests()
    {
        _fixture = new Fixture();
        _sut = new ShipRepository();
    }

    [Fact]
    public async Task Add_ShouldAddShipToRepository()
    {
        // Arrange
        var ship = _fixture.Create<Ship>();

        // Act
        await _sut.AddAsync(ship);

        // Assert
        Assert.Contains(ship, await _sut.GetAllAsync());
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectShip_WhenShipExists()
    {
        // Arrange
        var existingShip = _fixture.Create<Ship>();
        await _sut.AddAsync(existingShip);

        // Act
        var result = await _sut.GetByIdAsync(existingShip.Id);

        // Assert
        Assert.Equal(existingShip, result);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenShipDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _sut.GetByIdAsync(nonExistingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCode_ShouldReturnCorrectShip_WhenShipExists()
    {
        // Arrange
        var existingShip = _fixture.Create<Ship>();
        await _sut.AddAsync(existingShip);

        // Act
        var result = await _sut.GetByCodeAsync(existingShip.Code!);

        // Assert
        Assert.Equal(existingShip, result);
    }

    [Fact]
    public async Task GetByCode_ShouldReturnNull_WhenShipDoesNotExist()
    {
        // Arrange
        var nonExistingCode = "NonExistingCode";

        // Act
        var result = await _sut.GetByCodeAsync(nonExistingCode);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenShipDoesNotExist()
    {
        // Arrange
        var nonExistingShip = _fixture.Create<Ship>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.UpdateAsync(nonExistingShip));
    }

    [Fact]
    public async Task Delete_ShouldRemoveExistingShip_WhenShipExists()
    {
        // Arrange
        var existingShip = _fixture.Create<Ship>();
        await _sut.AddAsync(existingShip);

        // Act
        await _sut.DeleteAsync(existingShip.Id);

        // Assert
        var result = await _sut.GetByIdAsync(existingShip.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldThrowException_WhenShipDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.DeleteAsync(nonExistingId));
    }

    [Fact]
    public async Task IsCodeAlreadyExisted_ShouldReturnFalse_WhenCodeDoesNotExist()
    {
        // Arrange
        var existingShip = _fixture.Create<Ship>();
        await _sut.AddAsync(existingShip);

        var newShipWithDifferentCode = _fixture.Create<Ship>();

        // Act
        var result = await _sut.IsCodeAlreadyExistedAsync(existingShip.Id, newShipWithDifferentCode.Code!);

        // Assert
        Assert.False(result);
    }

}
