using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using ShipManagement.Api.Filters;
using ShipManagement.Application;
using ShipManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication().AddInfrastructure();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    var apiVersioningBuilder = builder.Services.AddApiVersioning(o =>
    {
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.ReportApiVersions = true;
        o.ApiVersionReader = ApiVersionReader.Combine(
            new QueryStringApiVersionReader("api-version"),
            new HeaderApiVersionReader("X-Version"),
            new MediaTypeApiVersionReader("ver"));
    });

}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173"));
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}
