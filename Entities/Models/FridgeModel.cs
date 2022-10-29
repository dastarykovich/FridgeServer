using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("fridge_model")]
    public class FridgeModel
    {
        [Column("id", TypeName = "UNIQUEIDENTIFIER")]
        public Guid Id { get; set; }

        [Required]
        [Column("fridge_id", TypeName = "UNIQUEIDENTIFIER")]
        [ForeignKey(nameof(Fridge))]
        public Guid FridgeId { get; set; }

        [Required]
        [Column("name", TypeName = "nvarchar(MAX)")]
        public string? Name { get; set; }

        [Column("year", TypeName = "int")]
        public int Year { get; set; }

        public Fridge? Fridge { get; set; }
    }
}
