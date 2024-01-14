FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ShipManagement.Api/ShipManagement.Api.csproj", "ShipManagement.Api/"]
COPY ["src/ShipManagement.Application/ShipManagement.Application.csproj", "ShipManagement.Application/"]
COPY ["src/ShipManagement.Domain/ShipManagement.Domain.csproj", "ShipManagement.Domain/"]
COPY ["src/ShipManagement.Contracts/ShipManagement.Contracts.csproj", "ShipManagement.Contracts/"]
COPY ["src/ShipManagement.Infrastructure/ShipManagement.Infrastructure.csproj", "ShipManagement.Infrastructure/"]
RUN dotnet restore "ShipManagement.Api/ShipManagement.Api.csproj"
COPY . ../
WORKDIR /src/ShipManagement.Api
RUN dotnet build "ShipManagement.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5148
EXPOSE 5148
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShipManagement.Api.dll"]
