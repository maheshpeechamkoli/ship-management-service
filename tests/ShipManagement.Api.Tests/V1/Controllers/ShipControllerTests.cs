using AutoFixture;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using ShipManagement.Api.V1.Controllers;
using ShipManagement.Contracts.Ships;
using ShipManagement.Application.Services.Common;
using ShipManagement.Application.Services.ShipServices;
using System.Net;


namespace ShipManagement.Api.Tests.V1.Controllers;

public partial class ShipControllerTests
{

    private readonly IFixture _fixture;
    private readonly Mock<IShipService> _serviceMock;
    private readonly ShipController _sut;

    [GeneratedRegex(@"^[A-Za-z]{4}-[0-9]{4}-[A-Za-z]{1}[0-9]{1}$")]
    private static partial Regex CodeRegex();

    public ShipControllerTests()
    {
        _fixture = new Fixture();
        _serviceMock = _fixture.Freeze<Mock<IShipService>>();
        _sut = new ShipController(_serviceMock.Object);
    }

    [Fact]
    public async Task Create_ValidShipRequest_ReturnsOk()
    {
        // Arrange
        var invalidRequest = new ShipRequest("ValidShipName", 10.5, 8.2, "ABCD-1234-E5");

        // Mocking a success result from the service
        _serviceMock.Setup(x => x.Create(It.IsAny<ShipModelRequest>()))
                        .ReturnsAsync(ShipOperationResult.SuccessResult((int)HttpStatusCode.Created, "Ship created successfully"));

        // Act
        var result = await _sut.Create(invalidRequest);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_InValidShipRequest_ReturnsOk()
    {
        // Arrange
        var invalidRequest = new ShipRequest("ValidShipName", 10.5, 8.2, "ABCD-1234-E5");

        // Mocking a success result from the service
        _serviceMock.Setup(x => x.Create(It.IsAny<ShipModelRequest>()))
                        .ReturnsAsync(ShipOperationResult.FailureResult((int)HttpStatusCode.Conflict, "A ship with the same code already exists"));

        // Act
        var result = await _sut.Create(invalidRequest);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }


    [Theory]
    [InlineData("AAAA-1111-1A")]
    [InlineData("1234-5678-9A")]
    [InlineData("AAAA-1111-A1B")]
    [InlineData("AAAA-111-A1")]
    [InlineData("AAAA-1111-A")]
    [InlineData("AAAA-11111-A1")]
    [InlineData("AAAA-1111-A12")]
    [InlineData("AAAA-1111-A@")]
    [InlineData("AAAA-1111-AB")]
    [InlineData("AAAA1111A1")]
    [InlineData("AAAA-1111-A1-Extra")]
    public void Create_Code_WithInvalidFormat_ShouldFailValidation(string invalidCode)
    {
        // Arrange
        var invalidRequest = new ShipRequest("InvalidShip", 10.5, 8.2, invalidCode);

        // Act
        var validationResult = ValidateModel(invalidRequest, "code");

        // Assert
        Assert.False(validationResult);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("as")]
    [InlineData("invalid_ship_is_lot_in_in_the_failed_case")]
    public void Create_Name_WithInvalidFormat_ShouldFailValidation(string invalidCode)
    {
        // Arrange
        var invalidRequest = new ShipRequest(invalidCode, 101.5, 100.3, "ABCD-1234-E5");

        // Act
        var validationResult = ValidateModel(invalidRequest, "name");

        // Assert
        Assert.False(validationResult);
    }

    private static bool ValidateModel(ShipRequest model, string type)
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);
        if (isValid)
        {
            switch (type)
            {
                case "name":
                    isValid = model.Name != null && model.Name.Length >= 3 && model.Name.Length <= 20;
                    break;
                case "code":
                    isValid = CodeRegex().IsMatch(model.Code);
                    break;
            }
        }
        return isValid;
    }


}