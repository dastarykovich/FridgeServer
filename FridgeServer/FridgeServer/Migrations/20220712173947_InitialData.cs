using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeServer.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "fridge_model",
                columns: new[] { "id", "name", "year" },
                values: new object[,]
                {
                    { new Guid("14ac0bdd-7e08-4d49-a439-6c4634529d74"), "GA-B379SLUL", 2018 },
                    { new Guid("6a5845af-1c8b-4060-9f1c-70018a63cb09"), "FR-102V", 2017 },
                    { new Guid("d098d69c-ff78-4f2a-bf7f-c9d343898963"), "Serie 6 VitaFresh Plus KGN39AI32R", 2020 }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "default_quantity", "name" },
                values: new object[,]
                {
                    { new Guid("3080f5cf-523e-405c-80e4-75d468a1a94e"), 2, "Orange" },
                    { new Guid("394c70b6-53c7-4b0a-bf98-a1bbd7c3c5c5"), 1, "Milk" },
                    { new Guid("5c6d012b-dc74-4580-9716-141de40af83d"), 1, "Water" },
                    { new Guid("6f377e69-b477-463a-a874-763660787941"), 10, "Eggs" },
                    { new Guid("7ac83035-17e7-4a9d-89ef-a7e2b14e931b"), 3, "Apple" }
                });

            migrationBuilder.InsertData(
                table: "fridge",
                columns: new[] { "id", "model_id", "name", "owner_name" },
                values: new object[] { new Guid("2b544aed-49ad-41f4-9372-8f2d9a9e6956"), new Guid("d098d69c-ff78-4f2a-bf7f-c9d343898963"), "Bosch", "Alex" });

            migrationBuilder.InsertData(
                table: "fridge",
                columns: new[] { "id", "model_id", "name", "owner_name" },
                values: new object[] { new Guid("48691dc6-312c-48e5-bd0d-ff23c852b9b3"), new Guid("14ac0bdd-7e08-4d49-a439-6c4634529d74"), "LG", "Mike" });

            migrationBuilder.InsertData(
                table: "fridge",
                columns: new[] { "id", "model_id", "name", "owner_name" },
                values: new object[] { new Guid("50e1549e-0bc1-4f8d-a21e-648738e3ecb9"), new Guid("6a5845af-1c8b-4060-9f1c-70018a63cb09"), "VR", "Nikita" });

            migrationBuilder.InsertData(
                table: "fridge_products",
                columns: new[] { "id", "fridge_id", "product_id", "quantity" },
                values: new object[,]
                {
                    { new Guid("23ae3b61-067f-4c2a-88cb-11b6098712de"), new Guid("48691dc6-312c-48e5-bd0d-ff23c852b9b3"), new Guid("6f377e69-b477-463a-a874-763660787941"), 1 },
                    { new Guid("7ad906f6-5740-4bb8-a7b3-d70134cec431"), new Guid("2b544aed-49ad-41f4-9372-8f2d9a9e6956"), new Guid("7ac83035-17e7-4a9d-89ef-a7e2b14e931b"), 2 },
                    { new Guid("bd22f93c-252c-4cd9-b23e-6c7345804f9c"), new Guid("50e1549e-0bc1-4f8d-a21e-648738e3ecb9"), new Guid("3080f5cf-523e-405c-80e4-75d468a1a94e"), 0 },
                    { new Guid("d6cb8731-b5a4-4ca6-9895-cab735417d93"), new Guid("48691dc6-312c-48e5-bd0d-ff23c852b9b3"), new Guid("5c6d012b-dc74-4580-9716-141de40af83d"), 3 },
                    { new Guid("e47f4bc8-60d7-4e59-b4c3-af50b7f732dc"), new Guid("50e1549e-0bc1-4f8d-a21e-648738e3ecb9"), new Guid("394c70b6-53c7-4b0a-bf98-a1bbd7c3c5c5"), 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "fridge_products",
                keyColumn: "id",
                keyValue: new Guid("23ae3b61-067f-4c2a-88cb-11b6098712de"));

            migrationBuilder.DeleteData(
                table: "fridge_products",
                keyColumn: "id",
                keyValue: new Guid("7ad906f6-5740-4bb8-a7b3-d70134cec431"));

            migrationBuilder.DeleteData(
                table: "fridge_products",
                keyColumn: "id",
                keyValue: new Guid("bd22f93c-252c-4cd9-b23e-6c7345804f9c"));

            migrationBuilder.DeleteData(
                table: "fridge_products",
                keyColumn: "id",
                keyValue: new Guid("d6cb8731-b5a4-4ca6-9895-cab735417d93"));

            migrationBuilder.DeleteData(
                table: "fridge_products",
                keyColumn: "id",
                keyValue: new Guid("e47f4bc8-60d7-4e59-b4c3-af50b7f732dc"));

            migrationBuilder.DeleteData(
                table: "fridge",
                keyColumn: "id",
                keyValue: new Guid("2b544aed-49ad-41f4-9372-8f2d9a9e6956"));

            migrationBuilder.DeleteData(
                table: "fridge",
                keyColumn: "id",
                keyValue: new Guid("48691dc6-312c-48e5-bd0d-ff23c852b9b3"));

            migrationBuilder.DeleteData(
                table: "fridge",
                keyColumn: "id",
                keyValue: new Guid("50e1549e-0bc1-4f8d-a21e-648738e3ecb9"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("3080f5cf-523e-405c-80e4-75d468a1a94e"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("394c70b6-53c7-4b0a-bf98-a1bbd7c3c5c5"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("5c6d012b-dc74-4580-9716-141de40af83d"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("6f377e69-b477-463a-a874-763660787941"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("7ac83035-17e7-4a9d-89ef-a7e2b14e931b"));

            migrationBuilder.DeleteData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("14ac0bdd-7e08-4d49-a439-6c4634529d74"));

            migrationBuilder.DeleteData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("6a5845af-1c8b-4060-9f1c-70018a63cb09"));

            migrationBuilder.DeleteData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("d098d69c-ff78-4f2a-bf7f-c9d343898963"));
        }
    }
}
