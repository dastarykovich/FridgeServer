using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class FridgeModelForManipulationDto
    {
        [Required(ErrorMessage = "FridgeModel name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "FridgeModel age is a required field.")]
        [Range(1930, int.MaxValue, ErrorMessage = "Year is required and it can't be lower than 1930")]
        public int Year { get; set; }
    }
}
