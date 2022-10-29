using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    internal class FridgeConfiguration : IEntityTypeConfiguration<Fridge>
    {
        public void Configure(EntityTypeBuilder<Fridge> builder)
        {
            builder.HasData
                (
                new Fridge
                {
                    Id = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                    Name = "Minsk-3-92",
                    OwnerName = "Jack",
                   
                },
                new Fridge
                {
                    Id = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                    Name = "Minsk-3-93",
                    OwnerName = "Jack",
                    
                },
                new Fridge
                {
                    Id = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                    Name = "Zakat-530-6",
                    OwnerName = "John",
                    
                },
                new Fridge
                {
                    Id = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),
                    Name = "Life is Avesome-35-790",
                    OwnerName = "Richard",
                    
                },
                new Fridge
                {
                    Id = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),
                    Name = "Samcon 555-2012",
                    OwnerName = "Jessica",
                   
                },
                new Fridge
                {
                    Id = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),
                    Name = "Samson 543-0098",
                    
                }
                );
        }
    }
}