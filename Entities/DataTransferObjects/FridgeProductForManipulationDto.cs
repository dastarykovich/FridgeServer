using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public class FridgeProductForManipulationDto
{
    [Required(ErrorMessage = "Fridge product quantity is a required field.")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity is required and it can't be lower than 0")]
    public int Quantity { get; set; }
}