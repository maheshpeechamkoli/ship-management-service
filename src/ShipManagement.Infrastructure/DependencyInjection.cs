using Microsoft.Extensions.DependencyInjection;
using ShipManagement.Application.Interfaces.Persistence;
using ShipManagement.Infrastructure.Persistence;

namespace ShipManagement.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {
        service.AddScoped<IShipRepository, ShipRepository>();

        return service;
    }

}