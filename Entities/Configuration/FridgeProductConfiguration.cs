using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    internal class FridgeProductConfiguration : IEntityTypeConfiguration<FridgeProduct>
    {
        public void Configure(EntityTypeBuilder<FridgeProduct> builder)
        {
            builder.HasData
               (
               new FridgeProduct
               {
                   Id = new Guid("C25A81BE-7C84-41EA-8EFF-08C309C2D9DB"),
                   ProductId = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                   FridgeId = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                   Quantity = 0
               },
               new FridgeProduct
               {
                   Id = new Guid("CECDB562-9FFD-4C7E-BF61-2DB806BFAE0B"),
                   ProductId = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                   FridgeId = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                   Quantity = 2
               },
               new FridgeProduct
               {
                   Id = new Guid("BDE194D8-D5B2-4A5F-ABC8-0614F3A282E9"),
                   ProductId = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                   FridgeId = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                   Quantity = 0
               },
                 new FridgeProduct
                 {
                     Id = new Guid("F16B2B79-B008-4EC1-B85E-DABC5067DB97"),
                     ProductId = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     FridgeId = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                     Quantity = 12
                 },
               new FridgeProduct
               {
                   Id = new Guid("0D629689-F3FC-46CA-A9C5-EA7F09C937AC"),
                   ProductId = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                   FridgeId = new Guid("DA0FF40E-B6E2-4BD5-8AEA-09B4DC0E2FD2"),
                   Quantity = 12
               },




               //2
               new FridgeProduct
               {
                   Id = new Guid("F4DA4FDA-5F6D-4EB6-8FE9-950ED3FA88CA"),
                   ProductId = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                   FridgeId = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                   Quantity = 4
               },
               new FridgeProduct
               {
                   Id = new Guid("2BF7355F-70EA-4507-81EA-276D80079511"),
                   ProductId = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                   FridgeId = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                   Quantity = 7
               },
               new FridgeProduct
               {
                   Id = new Guid("AF639E0C-DB59-4D70-9892-546E02EBC5A6"),
                   ProductId = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                   FridgeId = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                   Quantity = 1
               },
                 new FridgeProduct
                 {
                     Id = new Guid("BC8CD3FC-E2C1-4A6C-8308-2262F8C3C646"),
                     ProductId = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     FridgeId = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                     Quantity = 0
                 },
               new FridgeProduct
               {
                   Id = new Guid("64F0B443-D2A1-4887-9E3B-4BAF24EF86A9"),
                   ProductId = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                   FridgeId = new Guid("DC4EE6C9-D1D9-49F7-B303-87BF677370E5"),
                   Quantity = 9
               },

               //3



               new FridgeProduct
               {
                   Id = new Guid("12A2425E-CE46-491F-A2B2-CB6946A1B947"),
                   ProductId = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                   FridgeId = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                   Quantity = 3
               },
               new FridgeProduct
               {
                   Id = new Guid("6C63BCFC-4F56-4F7C-9AB3-E52BC5AB2F12"),
                   ProductId = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                   FridgeId = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                   Quantity = 8
               },
               new FridgeProduct
               {
                   Id = new Guid("20758E2B-F948-43DF-ABC0-1C5F8F78DB2C"),
                   ProductId = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                   FridgeId = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                   Quantity = 1
               },
                 new FridgeProduct
                 {
                     Id = new Guid("4245A68C-9E6C-4DD3-B2E9-520A0A31E858"),
                     ProductId = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     FridgeId = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                     Quantity = 5
                 },
               new FridgeProduct
               {
                   Id = new Guid("49B40553-E7B5-404D-A17B-051C07F1ED5A"),
                   ProductId = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                   FridgeId = new Guid("52D93CFA-01B4-48FC-8DF4-D10CD4DC16C5"),
                   Quantity = 2
               },
               //4



               new FridgeProduct
               {
                   Id = new Guid("7B03EBA1-2E53-437E-8DB5-17FC63781DB2"),
                   ProductId = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                   FridgeId = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),
                   Quantity = 4
               },
               new FridgeProduct
               {
                   Id = new Guid("245C76B2-D351-4DAE-BA7F-5F8759088251"),
                   ProductId = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                   FridgeId = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),

                   Quantity = 0
               },
               new FridgeProduct
               {
                   Id = new Guid("2DB1F5BF-4EE2-4F48-96CF-8692B9C25386"),
                   ProductId = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                   FridgeId = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),

                   Quantity = 8
               },
                 new FridgeProduct
                 {
                     Id = new Guid("88FE0F6B-7C29-4CC2-8C73-660AB3ED1450"),
                     ProductId = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     FridgeId = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),
                     Quantity = 7
                 },
               new FridgeProduct
               {
                   Id = new Guid("BD4CAA58-2D24-48CD-811E-7C6DB7AE09DC"),
                   ProductId = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                   FridgeId = new Guid("5163522D-F7FC-422A-8F94-53CEC73FDB1B"),
                   Quantity = 1
               },


               //5

               new FridgeProduct
               {
                   Id = new Guid("54F0C572-37B7-4175-AE07-37DDC69890EA"),
                   ProductId = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                   FridgeId = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),
                   Quantity = 0
               },
               new FridgeProduct
               {
                   Id = new Guid("966C57CD-A166-4F31-A24D-3A295C10C3B0"),
                   ProductId = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                   FridgeId = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),

                   Quantity = 0
               },
               new FridgeProduct
               {
                   Id = new Guid("537437C0-E82E-4C8D-AAA1-D392DEE0C91C"),
                   ProductId = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                   FridgeId = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),

                   Quantity = 0
               },
                 new FridgeProduct
                 {
                     Id = new Guid("1815718F-68AE-40F0-B6C6-148D2A40379F"),
                     ProductId = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     FridgeId = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),
                     Quantity = 0
                 },
               new FridgeProduct
               {
                   Id = new Guid("097224A1-3B22-4C14-99B3-F52AE82C07FD"),
                   ProductId = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                   FridgeId = new Guid("078D1633-E04D-4779-99AA-9F136FD5D725"),
                   Quantity = 4
               },





               new FridgeProduct
               {
                   Id = new Guid("52A3760F-E0C9-4EA5-B873-40AC8664E132"),
                   ProductId = new Guid("ABD39727-EC5A-4668-B0B4-EF565FDD2B56"),
                   FridgeId = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),
                   Quantity = 0
               },
               new FridgeProduct
               {
                   Id = new Guid("DDFCB278-84CA-4697-A5BC-26D007195357"),
                   ProductId = new Guid("9801551F-96B6-4B05-8B6B-D48B47E9AFCE"),
                   FridgeId = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),

                   Quantity = 7
               },
               new FridgeProduct
               {
                   Id = new Guid("2B0B5EF4-3823-4A53-AB89-DB8BE180C93D"),
                   ProductId = new Guid("FEF09AE0-3216-4DA9-9B78-28075D573314"),
                   FridgeId = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),

                   Quantity = 1
               },
                 new FridgeProduct
                 {
                     Id = new Guid("E7556FE3-CF1D-4727-B651-AFCA7AF94214"),
                     ProductId = new Guid("71C77130-16CD-4EEC-A77A-DC16420CF403"),
                     FridgeId = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),
                     Quantity = 1
                 },
               new FridgeProduct
               {
                   Id = new Guid("68241A3D-AF08-426A-ACD8-8D844E3F3733"),
                   ProductId = new Guid("D28EEE52-2142-4B0D-95ED-59237DEEB414"),
                   FridgeId = new Guid("5E7B0FFB-EE66-4A47-9A5A-BB9E5649DC15"),
                   Quantity = 2
               }


               );
        }
    }
}
