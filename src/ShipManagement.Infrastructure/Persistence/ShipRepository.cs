

using ShipManagement.Application.Interfaces.Persistence;
using ShipManagement.Domain.Entities;

namespace ShipManagement.Infrastructure.Persistence;

public class ShipRepository : IShipRepository
{
    private static readonly List<Ship> _ships = [];
    public Task AddAsync(Ship ship)
    {
        _ships.Add(ship);
        return Task.CompletedTask;
    }

    public Task<List<Ship>> GetAllAsync()
    {
        return Task.FromResult(_ships.ToList());
    }

    public Task<Ship?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_ships.FirstOrDefault(ship => ship.Id == id));
    }

    public Task<Ship?> GetByCodeAsync(string code)
    {
        return Task.FromResult(_ships.FirstOrDefault(ship => ship.Code == code));
    }

    public Task UpdateAsync(Ship updatedShip)
    {
        var existingShip = _ships.FirstOrDefault(ship => ship.Id == updatedShip.Id);

        if (existingShip != null)
        {
            existingShip.Name = updatedShip.Name;
            existingShip.Length = updatedShip.Length;
            existingShip.Width = updatedShip.Width;
            existingShip.Code = updatedShip.Code;
        }
        else
        {
            throw new InvalidOperationException("Ship not found for update.");
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var shipToRemove = _ships.FirstOrDefault(ship => ship.Id == id);

        if (shipToRemove != null)
        {
            _ships.Remove(shipToRemove);
        }
        else
        {
            throw new InvalidOperationException("Ship not found for deletion.");
        }

        return Task.CompletedTask;
    }

    public Task<bool> IsCodeAlreadyExistedAsync(Guid id, string code)
    {
        return Task.FromResult(_ships.Any(ship => ship.Id != id && ship.Code == code));
    }

}