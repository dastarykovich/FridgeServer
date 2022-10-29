using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("fridge")]
    public class Fridge
    {
        [Column("id", TypeName = "UNIQUEIDENTIFIER")]
        public Guid Id { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(MAX)")]
        public string? Name { get; set; }

        [Column("owner_name", TypeName = "nvarchar(MAX)")]
        public string? OwnerName { get; set; }

        public ICollection<FridgeModel>? FridgeModels { get; set; }

        public ICollection<FridgeProduct>? FridgeProducts { get; set; }
    }
}
