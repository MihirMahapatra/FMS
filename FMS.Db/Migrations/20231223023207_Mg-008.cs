using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "LabourRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabourRates_Fk_ProductTypeId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourRates_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "LabourRates",
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
                name: "FK_LabourRates_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropIndex(
                name: "IX_LabourRates_Fk_ProductTypeId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropColumn(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "LabourRates");
        }
    }
}
