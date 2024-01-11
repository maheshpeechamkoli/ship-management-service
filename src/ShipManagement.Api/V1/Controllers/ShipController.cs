using Microsoft.AspNetCore.Mvc;
using ShipManagement.Application.Services.Common;
using ShipManagement.Application.Services.ShipServices;
using ShipManagement.Contracts.Ships;

namespace ShipManagement.Api.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/ship")]
[ApiVersion("1.0")]
public class ShipController(IShipService shipService) : ControllerBase
{
    public readonly IShipService _shipService = shipService ?? throw new ArgumentNullException(nameof(shipService));

    /// <summary>
    /// Creates a new ship based on the provided ship request.
    /// </summary>
    /// <remarks>
    /// Endpoint for creating a new ship with the specified details.
    /// </remarks>
    /// <param name="request">The ship request containing ship details such as name, length, width, and code.</param>
    /// <returns>
    /// <para>Returns 200 OK if the ship creation is successful, along with details of the created ship.</para>
    /// <para>Returns 400 Bad Request if there are validation errors or the ship creation fails, along with an error message.</para>
    /// </returns>
    /// <response code="200">Returns the details of the created ship when the ship creation is successful.</response>
    /// <response code="400">Returns an error message if there are validation errors or the ship creation fails.</response>
    [HttpPost("create")]
    public async Task<IActionResult> Create(ShipRequest request)
    {
        var shipRequest = new ShipModelRequest
        {
            Name = request.Name,
            Length = request.Length,
            Width = request.Width,
            Code = request.Code
        };
        var shipResult = await _shipService.Create(shipRequest);

        if (shipResult.Success)
            return Ok(shipResult);

        return BadRequest(shipResult.Message);
    }

    /// <summary>
    /// Get a list of all available ships.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all ships stored in the system.
    /// </remarks>
    /// <returns>
    /// <para>Returns a list of ships when successful (Status 200 OK).</para>
    /// <para>Returns Not Found (Status 404) if no ships are available.</para>
    /// </returns>
    /// <response code="200">Returns the list of all ships when successful.</response>
    /// <response code="404">Returns Not Found if no ships are available.</response>
    [HttpGet("list")]
    public async Task<IActionResult> GetAllShips()
    {
        var response = await _shipService.GetAllShips();
        //return Ok(allShips);
        return response != null ? Ok(response) : NotFound();
    }

    /// <summary>
    /// Update details for a specific ship.
    /// </summary>
    /// <remarks>
    /// This endpoint allows updating details for a specific ship based on the provided ship ID.
    /// </remarks>
    /// <param name="id">The unique identifier of the ship to be updated.</param>
    /// <param name="request">The ship details to be updated, including name, length, width, and code.</param>
    /// <returns>
    /// <para>Returns 200 OK if the ship update is successful, along with updated ship details.</para>
    /// <para>Returns 400 Bad Request if the ship update fails, along with an error message.</para>
    /// </returns>
    /// <response code="200">Returns the updated ship details when the update is successful.</response>
    /// <response code="400">Returns an error message if the ship update fails.</response>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(Guid id, ShipRequest request)
    {
        var shipRequest = new ShipModelRequest
        {
            Name = request.Name,
            Length = request.Length,
            Width = request.Width,
            Code = request.Code
        };
        var updateResult = await _shipService.Update(id, shipRequest);

        if (updateResult.Success)
        {
            return Ok(updateResult);
        }

        return BadRequest(updateResult);
    }

    /// <summary>
    /// Delete a specific ship from the system.
    /// </summary>
    /// <remarks>
    /// This endpoint allows deleting a specific ship based on the provided ship ID.
    /// </remarks>
    /// <param name="id">The unique identifier of the ship to be deleted.</param>
    /// <returns>
    /// <para>Returns 200 OK if the ship deletion is successful.</para>
    /// <para>Returns 400 Bad Request if the ship deletion fails, along with an error message.</para>
    /// </returns>
    /// <response code="200">Returns success when the ship deletion is successful.</response>
    /// <response code="400">Returns an error message if the ship deletion fails.</response>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResult = await _shipService.Delete(id);

        if (deleteResult.Success)
        {
            return Ok(deleteResult);
        }

        return BadRequest(deleteResult);
    }

}

