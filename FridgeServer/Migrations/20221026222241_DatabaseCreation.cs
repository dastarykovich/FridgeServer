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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fridge",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    owner_name = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fridge", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    default_quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fridge_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    fridge_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fridge_model", x => x.id);
                    table.ForeignKey(
                        name: "FK_fridge_model_fridge_fridge_id",
                        column: x => x.fridge_id,
                        principalTable: "fridge",
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3d028b1b-695f-40c7-a712-bb515a787c64", "53839e66-0a98-4f53-8b81-4ff4d55828f6", "Administrator", "ADMINISTRATOR" },
                    { "ee916832-ff98-44c1-bfd6-32c50be6b2dc", "c8431e4a-6eb5-4f99-bb96-759ae353d1f5", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "fridge",
                columns: new[] { "id", "name", "owner_name" },
                values: new object[,]
                {
                    { new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), "Life is Avesome-35-790", "Richard" },
                    { new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), "Samcon 555-2012", "Jessica" },
                    { new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), "Zakat-530-6", "John" },
                    { new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), "Samson 543-0098", null },
                    { new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), "Minsk-3-92", "Jack" },
                    { new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), "Minsk-3-93", "Jack" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "default_quantity", "name" },
                values: new object[,]
                {
                    { new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 3, "Apple" },
                    { new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 1, "Cheese" },
                    { new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 2, "Milk" },
                    { new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 2, "Pelmeni" },
                    { new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 5, "Coke" }
                });

            migrationBuilder.InsertData(
                table: "fridge_model",
                columns: new[] { "id", "fridge_id", "name", "year" },
                values: new object[,]
                {
                    { new Guid("1eb04337-c367-4626-a831-0b44e39ce68d"), new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), "LA-35", 2017 },
                    { new Guid("34c233a2-ef55-4353-8c41-01ed03be2213"), new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), "Zakat-530", 1990 },
                    { new Guid("6e529b9f-bb5e-4552-9038-653feef2a72b"), new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), "Samson-555", 2021 },
                    { new Guid("7a4b06ea-f42f-4579-8ea3-4a9191b2c5f8"), new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), "Samson-543", 2022 },
                    { new Guid("f06e64b5-81e8-4b8e-8636-00f7d298bc65"), new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), "Minsk-3", 2012 },
                    { new Guid("fabe64b5-81e8-4b8e-8636-00f7d298bc65"), new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), "Minsk-3.1", 2012 }
                });

            migrationBuilder.InsertData(
                table: "fridge_products",
                columns: new[] { "id", "fridge_id", "product_id", "quantity" },
                values: new object[,]
                {
                    { new Guid("097224a1-3b22-4c14-99b3-f52ae82c07fd"), new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 4 },
                    { new Guid("0d629689-f3fc-46ca-a9c5-ea7f09c937ac"), new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 12 },
                    { new Guid("12a2425e-ce46-491f-a2b2-cb6946a1b947"), new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 3 },
                    { new Guid("1815718f-68ae-40f0-b6c6-148d2a40379f"), new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 0 },
                    { new Guid("20758e2b-f948-43df-abc0-1c5f8f78db2c"), new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 1 },
                    { new Guid("245c76b2-d351-4dae-ba7f-5f8759088251"), new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 0 },
                    { new Guid("2b0b5ef4-3823-4a53-ab89-db8be180c93d"), new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 1 },
                    { new Guid("2bf7355f-70ea-4507-81ea-276d80079511"), new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 7 },
                    { new Guid("2db1f5bf-4ee2-4f48-96cf-8692b9c25386"), new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 8 },
                    { new Guid("4245a68c-9e6c-4dd3-b2e9-520a0a31e858"), new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 5 },
                    { new Guid("49b40553-e7b5-404d-a17b-051c07f1ed5a"), new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 2 },
                    { new Guid("52a3760f-e0c9-4ea5-b873-40ac8664e132"), new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 0 },
                    { new Guid("537437c0-e82e-4c8d-aaa1-d392dee0c91c"), new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 0 },
                    { new Guid("54f0c572-37b7-4175-ae07-37ddc69890ea"), new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 0 },
                    { new Guid("64f0b443-d2a1-4887-9e3b-4baf24ef86a9"), new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 9 },
                    { new Guid("68241a3d-af08-426a-acd8-8d844e3f3733"), new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 2 },
                    { new Guid("6c63bcfc-4f56-4f7c-9ab3-e52bc5ab2f12"), new Guid("52d93cfa-01b4-48fc-8df4-d10cd4dc16c5"), new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 8 },
                    { new Guid("7b03eba1-2e53-437e-8db5-17fc63781db2"), new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 4 },
                    { new Guid("88fe0f6b-7c29-4cc2-8c73-660ab3ed1450"), new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 7 },
                    { new Guid("966c57cd-a166-4f31-a24d-3a295c10c3b0"), new Guid("078d1633-e04d-4779-99aa-9f136fd5d725"), new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 0 },
                    { new Guid("af639e0c-db59-4d70-9892-546e02ebc5a6"), new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 1 },
                    { new Guid("bc8cd3fc-e2c1-4a6c-8308-2262f8c3c646"), new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 0 },
                    { new Guid("bd4caa58-2d24-48cd-811e-7c6db7ae09dc"), new Guid("5163522d-f7fc-422a-8f94-53cec73fdb1b"), new Guid("d28eee52-2142-4b0d-95ed-59237deeb414"), 1 },
                    { new Guid("bde194d8-d5b2-4a5f-abc8-0614f3a282e9"), new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), new Guid("fef09ae0-3216-4da9-9b78-28075d573314"), 0 },
                    { new Guid("c25a81be-7c84-41ea-8eff-08c309c2d9db"), new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 0 },
                    { new Guid("cecdb562-9ffd-4c7e-bf61-2db806bfae0b"), new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 2 },
                    { new Guid("ddfcb278-84ca-4697-a5bc-26d007195357"), new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), new Guid("9801551f-96b6-4b05-8b6b-d48b47e9afce"), 7 },
                    { new Guid("e7556fe3-cf1d-4727-b651-afca7af94214"), new Guid("5e7b0ffb-ee66-4a47-9a5a-bb9e5649dc15"), new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 1 },
                    { new Guid("f16b2b79-b008-4ec1-b85e-dabc5067db97"), new Guid("da0ff40e-b6e2-4bd5-8aea-09b4dc0e2fd2"), new Guid("71c77130-16cd-4eec-a77a-dc16420cf403"), 12 },
                    { new Guid("f4da4fda-5f6d-4eb6-8fe9-950ed3fa88ca"), new Guid("dc4ee6c9-d1d9-49f7-b303-87bf677370e5"), new Guid("abd39727-ec5a-4668-b0b4-ef565fdd2b56"), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_fridge_model_fridge_id",
                table: "fridge_model",
                column: "fridge_id");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "fridge_model");

            migrationBuilder.DropTable(
                name: "fridge_products");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "fridge");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
