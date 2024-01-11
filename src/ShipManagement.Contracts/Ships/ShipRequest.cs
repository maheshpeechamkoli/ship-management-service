using System.ComponentModel.DataAnnotations;

namespace ShipManagement.Contracts.Ships
{
    public record ShipRequest
    (
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters.")]
        string Name,

        [Required(ErrorMessage = "Length is required.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Length must be a valid double.")]
        double Length,

        [Required(ErrorMessage = "Width is required.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Width must be a valid double.")]
        double Width,

        [Required(ErrorMessage = "Code is required.")]
        [RegularExpression(@"^[A-Za-z]{4}-[0-9]{4}-[A-Za-z]{1}[0-9]{1}$", ErrorMessage = "Invalid Code format.")]
        string Code
    );
}
