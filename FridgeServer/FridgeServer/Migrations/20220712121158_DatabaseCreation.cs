using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeServer.Migrations
{
    public partial class DatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fridge_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fridge_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    default_quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fridge",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    owner_name = table.Column<string>(type: "text", nullable: false),
                    model_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fridge", x => x.id);
                    table.ForeignKey(
                        name: "FK_fridge_fridge_model_model_id",
                        column: x => x.model_id,
                        principalTable: "fridge_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fridge_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    product_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    fridge_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fridge_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_fridge_products_fridge_fridge_id",
                        column: x => x.fridge_id,
                        principalTable: "fridge",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fridge_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fridge_model_id",
                table: "fridge",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_fridge_products_fridge_id",
                table: "fridge_products",
                column: "fridge_id");

            migrationBuilder.CreateIndex(
                name: "IX_fridge_products_product_id",
                table: "fridge_products",
                column: "product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fridge_products");

            migrationBuilder.DropTable(
                name: "fridge");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "fridge_model");
        }
    }
}
