using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeServer.Migrations
{
    public partial class RemovingModelIdAndAddingFridgeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fridge_fridge_model_model_id",
                table: "fridge");

            migrationBuilder.DropIndex(
                name: "IX_fridge_model_id",
                table: "fridge");

            migrationBuilder.DropColumn(
                name: "model_id",
                table: "fridge");

            migrationBuilder.AddColumn<Guid>(
                name: "fridge_id",
                table: "fridge_model",
                type: "UNIQUEIDENTIFIER",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("14ac0bdd-7e08-4d49-a439-6c4634529d74"),
                column: "fridge_id",
                value: new Guid("48691dc6-312c-48e5-bd0d-ff23c852b9b3"));

            migrationBuilder.UpdateData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("6a5845af-1c8b-4060-9f1c-70018a63cb09"),
                column: "fridge_id",
                value: new Guid("50e1549e-0bc1-4f8d-a21e-648738e3ecb9"));

            migrationBuilder.UpdateData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("d098d69c-ff78-4f2a-bf7f-c9d343898963"),
                columns: new[] { "fridge_id", "name" },
                values: new object[] { new Guid("2b544aed-49ad-41f4-9372-8f2d9a9e6956"), "KGV39XW2AR" });

            migrationBuilder.CreateIndex(
                name: "IX_fridge_model_fridge_id",
                table: "fridge_model",
                column: "fridge_id");

            migrationBuilder.AddForeignKey(
                name: "FK_fridge_model_fridge_fridge_id",
                table: "fridge_model",
                column: "fridge_id",
                principalTable: "fridge",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fridge_model_fridge_fridge_id",
                table: "fridge_model");

            migrationBuilder.DropIndex(
                name: "IX_fridge_model_fridge_id",
                table: "fridge_model");

            migrationBuilder.DropColumn(
                name: "fridge_id",
                table: "fridge_model");

            migrationBuilder.AddColumn<Guid>(
                name: "model_id",
                table: "fridge",
                type: "UNIQUEIDENTIFIER",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "fridge",
                keyColumn: "id",
                keyValue: new Guid("2b544aed-49ad-41f4-9372-8f2d9a9e6956"),
                column: "model_id",
                value: new Guid("d098d69c-ff78-4f2a-bf7f-c9d343898963"));

            migrationBuilder.UpdateData(
                table: "fridge",
                keyColumn: "id",
                keyValue: new Guid("48691dc6-312c-48e5-bd0d-ff23c852b9b3"),
                column: "model_id",
                value: new Guid("14ac0bdd-7e08-4d49-a439-6c4634529d74"));

            migrationBuilder.UpdateData(
                table: "fridge",
                keyColumn: "id",
                keyValue: new Guid("50e1549e-0bc1-4f8d-a21e-648738e3ecb9"),
                column: "model_id",
                value: new Guid("6a5845af-1c8b-4060-9f1c-70018a63cb09"));

            migrationBuilder.UpdateData(
                table: "fridge_model",
                keyColumn: "id",
                keyValue: new Guid("d098d69c-ff78-4f2a-bf7f-c9d343898963"),
                column: "name",
                value: "Serie 6 VitaFresh Plus KGN39AI32R");

            migrationBuilder.CreateIndex(
                name: "IX_fridge_model_id",
                table: "fridge",
                column: "model_id");

            migrationBuilder.AddForeignKey(
                name: "FK_fridge_fridge_model_model_id",
                table: "fridge",
                column: "model_id",
                principalTable: "fridge_model",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
