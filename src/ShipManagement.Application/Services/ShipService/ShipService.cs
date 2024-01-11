using ShipManagement.Application.Interfaces.Persistence;
using ShipManagement.Application.Services.Common;
using ShipManagement.Domain.Entities;


namespace ShipManagement.Application.Services.ShipServices;

public class ShipService(IShipRepository shipRepository) : IShipService
{
    public readonly IShipRepository _shipRepository = shipRepository ?? throw new ArgumentNullException(nameof(shipRepository));

    public async Task<ShipOperationResult> Create(ShipModelRequest shipRequest)
    {
        // Check if a ship with the same code already exists
        var existingShip = await _shipRepository.GetByCodeAsync(shipRequest.Code);
        if (existingShip != null)
        {
            return ShipOperationResult.FailureResult("A ship with the same code already exists");
        }

        // If no duplicate, proceed with creating the ship
        var ship = new Ship
        {
            Name = shipRequest.Name,
            Length = shipRequest.Length,
            Width = shipRequest.Width,
            Code = shipRequest.Code
        };
        await _shipRepository.AddAsync(ship);

        return ShipOperationResult.SuccessResult("Ship created successfully");
    }

    public Task<List<Ship>> GetAllShips()
    {
        return _shipRepository.GetAllAsync();
    }

    public async Task<ShipOperationResult> Update(Guid id, ShipModelRequest shipRequest)
    {
        var existingShip = await _shipRepository.GetByIdAsync(id);

        if (existingShip != null)
        {
            // Check if a ship with the same code already exists expect with this id
            if (await _shipRepository.IsCodeAlreadyExistedAsync(existingShip.Id, shipRequest.Code))
            {
                return ShipOperationResult.FailureResult("A ship with the same code already exists");
            }

            existingShip.Name = shipRequest.Name;
            existingShip.Length = shipRequest.Length;
            existingShip.Width = shipRequest.Width;
            existingShip.Code = shipRequest.Code;

            await _shipRepository.UpdateAsync(existingShip);

            return ShipOperationResult.SuccessResult("Ship updated successfully");
        }

        return ShipOperationResult.FailureResult("Ship not found for update");
    }

    public async Task<ShipOperationResult> Delete(Guid id)
    {
        var shipToDelete = await _shipRepository.GetByIdAsync(id);

        if (shipToDelete != null)
        {
            await _shipRepository.DeleteAsync(id);
            return ShipOperationResult.SuccessResult("Ship deleted successfully");
        }

        return ShipOperationResult.FailureResult("Ship not found for deletion");
    }


}