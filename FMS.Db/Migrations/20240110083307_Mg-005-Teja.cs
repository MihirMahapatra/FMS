using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg005Teja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "LabourRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabourRates_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourRates_Branches_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_BranchId",
                principalSchema: "dbo",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabourRates_Branches_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropIndex(
                name: "IX_LabourRates_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropColumn(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "LabourRates");
        }
    }
}
