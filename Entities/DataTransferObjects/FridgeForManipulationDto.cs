using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class FridgeForManipulationDto
    {
        [Required(ErrorMessage = "Fridge name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Fridge owner name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the OwnerName is 60 characters.")]
        public string? OwnerName { get; set; }

        public IEnumerable<FridgeModelForCreationDto>? FridgeModels { get; set; }
    }
}
