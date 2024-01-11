namespace ShipManagement.Application.Services.Common;
public class ShipModelRequest
{
    public required string Name { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public required string Code { get; set; }
}