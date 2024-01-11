using Microsoft.Extensions.DependencyInjection;
using ShipManagement.Application.Services.ShipServices;

namespace ShipManagement.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IShipService, ShipService>();

        return service;
    }

}