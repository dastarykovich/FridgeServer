using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    internal class FridgeModelConfiguration : IEntityTypeConfiguration<FridgeModel>
    {
        public void Configure(EntityTypeBuilder<FridgeModel> builder)
        {
            builder.HasData
                (
                new FridgeModel
                {
                    Id = new Guid("F06E64B5-81E8-4B8E-8636-00F7D298BC65"),
                    Name = "Minsk-3",
                    FridgeId = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                    Year = 2012
                },
                 new FridgeModel
                 {
                     Id = new Guid("FABE64B5-81E8-4B8E-8636-00F7D298BC65"),
                     Name = "Minsk-3.1",
                     FridgeId = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                     Year = 2012
                 },
                new FridgeModel
                {
                    Id = new Guid("34C233A2-EF55-4353-8C41-01ED03BE2213"),
                    Name = "Zakat-530",
                    FridgeId = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                    Year = 1990
                },
                new FridgeModel
                {
                    Id = new Guid("1EB04337-C367-4626-A831-0B44E39CE68D"),
                    Name = "LA-35",
                    FridgeId = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),
                    Year = 2017
                },
                new FridgeModel
                {
                    Id = new Guid("6E529B9F-BB5E-4552-9038-653FEEF2A72B"),
                    Name = "Samson-555",
                    FridgeId = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),
                    Year = 2021
                },
                new FridgeModel
                {
                    Id = new Guid("7A4B06EA-F42F-4579-8EA3-4A9191B2C5F8"),
                    Name = "Samson-543",
                    FridgeId = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),
                    Year = 2022
                }
                );
        }
    }
}