using ShipManagement.Domain.Entities;

namespace ShipManagement.Application.Interfaces.Persistence;

public interface IShipRepository
{
    Task AddAsync(Ship ship);
    Task<List<Ship>> GetAllAsync();
    Task<Ship?> GetByIdAsync(Guid id);
    Task<Ship?> GetByCodeAsync(string code);
    Task UpdateAsync(Ship updatedShip);
    Task DeleteAsync(Guid id);
    Task<bool> IsCodeAlreadyExistedAsync(Guid id, string code);
}