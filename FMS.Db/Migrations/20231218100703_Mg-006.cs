using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlternateUnits",
                schema: "dbo",
                columns: table => new
                {
                    AlternateUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AlternateUnitName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlternateQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FK_ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fk_UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternateUnits", x => x.AlternateUnitId);
                    table.ForeignKey(
                        name: "FK_AlternateUnits_Products_FK_ProductId",
                        column: x => x.FK_ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlternateUnits_Units_Fk_UnitId",
                        column: x => x.Fk_UnitId,
                        principalSchema: "dbo",
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Fk_ProductTypeId",
                schema: "dbo",
                table: "Groups",
                column: "Fk_ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternateUnits_FK_ProductId",
                schema: "dbo",
                table: "AlternateUnits",
                column: "FK_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternateUnits_Fk_UnitId",
                schema: "dbo",
                table: "AlternateUnits",
                column: "Fk_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "Groups",
                column: "Fk_ProductTypeId",
                principalSchema: "dbo",
                principalTable: "ProductTypes",
                principalColumn: "ProductTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "AlternateUnits",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Fk_ProductTypeId",
                schema: "dbo",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "Groups");
        }
    }
}
