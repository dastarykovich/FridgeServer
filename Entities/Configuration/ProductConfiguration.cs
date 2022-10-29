using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
                 (
                 new Product
                 {
                     Id = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                     Name = "Milk",
                     DefaultQuantity = 2
                 },
                 new Product
                 {
                     Id = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                     Name = "Cheese",
                     DefaultQuantity = 1
                 },
                 new Product
                 {
                     Id = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                     Name = "Coke",
                     DefaultQuantity = 5
                 },
                 new Product
                 {
                     Id = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                     Name = "Pelmeni",
                     DefaultQuantity = 2
                 },

                 new Product
                 {
                     Id = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     Name = "Apple",
                     DefaultQuantity = 3
                 }
                 );
        }
    }
}

