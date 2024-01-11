using System.ComponentModel.DataAnnotations;

namespace ShipManagement.Domain.Entities;

public class Ship
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string? Name { get; set; }
    [Required]
    public double Length { get; set; }
    [Required]
    public double Width { get; set; }
    [Required]
    public string? Code { get; set; }
}