using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("fridge_products")]
    public class FridgeProduct
    {
        [Column("id", TypeName = "UNIQUEIDENTIFIER")]
        public Guid Id { get; set; }

        [Required]
        [Column("product_id", TypeName = "UNIQUEIDENTIFIER")]
        [ForeignKey(nameof(Products))]
        public Guid ProductId { get; set; }

        [Required]
        [Column("fridge_id", TypeName = "UNIQUEIDENTIFIER")]
        [ForeignKey(nameof(Fridge))]
        public Guid FridgeId { get; set; }

        [Required]
        [Column("quantity", TypeName = "int")]
        public int Quantity { get; set; }

        public Fridge? Fridge { get; set; }

        public Product? Products { get; set; }
    }
}
