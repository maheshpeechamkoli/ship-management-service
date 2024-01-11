
namespace ShipManagement.Application.Services.ShipServices;

using ShipManagement.Application.Services.Common;
using ShipManagement.Domain.Entities;
public interface IShipService
{
    Task<ShipOperationResult> Create(ShipModelRequest shipRequest);
    Task<List<Ship>> GetAllShips();

    Task<ShipOperationResult> Update(Guid id, ShipModelRequest shipRequest);
    Task<ShipOperationResult> Delete(Guid id);
}

