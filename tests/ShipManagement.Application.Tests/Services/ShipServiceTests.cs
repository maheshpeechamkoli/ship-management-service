

using AutoFixture;
using Moq;
using ShipManagement.Application.Interfaces.Persistence;
using ShipManagement.Application.Services.Common;
using ShipManagement.Application.Services.ShipServices;
using ShipManagement.Domain.Entities;

namespace ShipManagement.Application.Tests.Services;

public class ShipServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IShipRepository> _repositoryMock;
    private readonly ShipService _sut;

    public ShipServiceTests()
    {
        _fixture = new Fixture();
        _repositoryMock = _fixture.Freeze<Mock<IShipRepository>>();
        _sut = new ShipService(_repositoryMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnSuccessResult_WhenNoDuplicateShipCode()
    {
        // Arrange
        var shipRequest = new ShipModelRequest
        {
            Name = "TestShip",
            Length = 100,
            Width = 20,
            Code = "ABC123"
        };

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(default(Ship));

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Create(shipRequest);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Ship created successfully", result.Message);

        mockShipRepository.Verify(repo => repo.GetByCodeAsync(shipRequest.Code), Times.Once);

        mockShipRepository.Verify(repo => repo.AddAsync(It.IsAny<Ship>()), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldReturnFailureResult_WhenDuplicateShipCode()
    {
        // Arrange
        var shipRequest = new ShipModelRequest
        {
            Name = "TestShip",
            Length = 100,
            Width = 20,
            Code = "ABC123"
        };

        var existingShip = new Ship
        {
            Name = "ExistingShip",
            Length = 90,
            Width = 18,
            Code = "ABC123"
        };

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByCodeAsync(shipRequest.Code)).ReturnsAsync(existingShip);

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Create(shipRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("A ship with the same code already exists", result.Message);

        mockShipRepository.Verify(repo => repo.GetByCodeAsync(shipRequest.Code), Times.Once);
        mockShipRepository.Verify(repo => repo.AddAsync(It.IsAny<Ship>()), Times.Never);
    }

    [Fact]
    public async Task GetAllShips_ShouldReturnListOfShips()
    {
        // Arrange
        var mockShipRepository = new Mock<IShipRepository>();

        var expectedShips = new List<Ship>
        {
            new Ship { Name = "Ship1", Length = 100, Width = 20, Code = "AAAA-1111-1A" },
            new Ship { Name = "Ship2", Length = 120, Width = 25, Code = "AAAA-1121-1A" },
        };

        mockShipRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedShips);

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.GetAllShips();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Ship>>(result);
        Assert.Equal(expectedShips.Count, result.Count);


        Assert.Equal(expectedShips[0].Name, result[0].Name);
        Assert.Equal(expectedShips[0].Length, result[0].Length);
        Assert.Equal(expectedShips[0].Width, result[0].Width);
        Assert.Equal(expectedShips[0].Code, result[0].Code);


        mockShipRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnSuccessResult_WhenShipExistsAndCodeIsUnique()
    {
        // Arrange
        Guid existingShipId = Guid.NewGuid();
        var shipRequest = new ShipModelRequest
        {
            Name = "UpdatedShip",
            Length = 150,
            Width = 30,
            Code = "NEW-1234-1A"
        };

        var existingShip = new Ship
        {
            Id = existingShipId,
            Name = "ExistingShip",
            Length = 100,
            Width = 20,
            Code = "OLD-5678-1B"
        };

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByIdAsync(existingShipId)).ReturnsAsync(existingShip);
        mockShipRepository.Setup(repo => repo.IsCodeAlreadyExistedAsync(existingShipId, shipRequest.Code)).ReturnsAsync(false);

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Update(existingShipId, shipRequest);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Ship updated successfully", result.Message);

        mockShipRepository.Verify(repo => repo.GetByIdAsync(existingShipId), Times.Once);
        mockShipRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Ship>()), Times.Once);
        mockShipRepository.Verify(repo => repo.IsCodeAlreadyExistedAsync(existingShipId, shipRequest.Code), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnFailureResult_WhenShipNotFound()
    {
        // Arrange
        Guid nonExistingShipId = Guid.NewGuid();
        var shipRequest = new ShipModelRequest
        {
            Name = "UpdatedShip",
            Length = 150,
            Width = 30,
            Code = "NEW-1234-1A"
        };

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(default(Ship));

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Update(nonExistingShipId, shipRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Ship not found for update", result.Message);

        mockShipRepository.Verify(repo => repo.GetByIdAsync(nonExistingShipId), Times.Once);
        mockShipRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Ship>()), Times.Never);
        mockShipRepository.Verify(repo => repo.IsCodeAlreadyExistedAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Update_ShouldReturnFailureResult_WhenCodeAlreadyExists()
    {
        // Arrange
        Guid existingShipId = Guid.NewGuid();
        var shipRequest = new ShipModelRequest
        {
            Name = "UpdatedShip",
            Length = 150,
            Width = 30,
            Code = "DUPLICATE-CODE"
        };

        var existingShip = new Ship
        {
            Id = existingShipId,
            Name = "ExistingShip",
            Length = 100,
            Width = 20,
            Code = "OLD-CODE"
        };

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByIdAsync(existingShipId)).ReturnsAsync(existingShip);
        mockShipRepository.Setup(repo => repo.IsCodeAlreadyExistedAsync(existingShipId, shipRequest.Code)).ReturnsAsync(true);

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Update(existingShipId, shipRequest);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("A ship with the same code already exists", result.Message);

        mockShipRepository.Verify(repo => repo.GetByIdAsync(existingShipId), Times.Once);
        mockShipRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Ship>()), Times.Never);
        mockShipRepository.Verify(repo => repo.IsCodeAlreadyExistedAsync(existingShipId, shipRequest.Code), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnSuccessResult_WhenShipExists()
    {
        // Arrange
        Guid existingShipId = Guid.NewGuid();

        var existingShip = new Ship
        {
            Id = existingShipId,
            Name = "ExistingShip",
            Length = 100,
            Width = 20,
            Code = "OLD-CODE"
        };

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByIdAsync(existingShipId)).ReturnsAsync(existingShip);

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Delete(existingShipId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Ship deleted successfully", result.Message);

        mockShipRepository.Verify(repo => repo.GetByIdAsync(existingShipId), Times.Once);
        mockShipRepository.Verify(repo => repo.DeleteAsync(existingShipId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailureResult_WhenShipNotFound()
    {
        // Arrange
        Guid nonExistingShipId = Guid.NewGuid();

        var mockShipRepository = new Mock<IShipRepository>();
        mockShipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Ship));

        var shipService = new ShipService(mockShipRepository.Object);

        // Act
        var result = await shipService.Delete(nonExistingShipId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Ship not found for deletion", result.Message);

        mockShipRepository.Verify(repo => repo.GetByIdAsync(nonExistingShipId), Times.Once);
        mockShipRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }




}
