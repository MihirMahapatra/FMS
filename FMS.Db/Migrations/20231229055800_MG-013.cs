using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class MG013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabourRates_Branches_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropForeignKey(
                name: "FK_LabourRates_FinancialYears_Fk_FinancialYearId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropIndex(
                name: "IX_LabourRates_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropIndex(
                name: "IX_LabourRates_Fk_FinancialYearId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropColumn(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.DropColumn(
                name: "Fk_FinancialYearId",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.AddColumn<string>(
                name: "FinancialYear",
                schema: "dbo",
                table: "LabourRates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialYear",
                schema: "dbo",
                table: "LabourRates");

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "LabourRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_FinancialYearId",
                schema: "dbo",
                table: "LabourRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabourRates_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_LabourRates_Fk_FinancialYearId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_FinancialYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourRates_Branches_Fk_BranchId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_BranchId",
                principalSchema: "dbo",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabourRates_FinancialYears_Fk_FinancialYearId",
                schema: "dbo",
                table: "LabourRates",
                column: "Fk_FinancialYearId",
                principalSchema: "dbo",
                principalTable: "FinancialYears",
                principalColumn: "FinancialYearId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
