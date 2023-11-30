using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg004Tej : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "dbo",
                table: "CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "CompanyDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDetails_Fk_BranchId",
                schema: "dbo",
                table: "CompanyDetails",
                column: "Fk_BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDetails_Branches_Fk_BranchId",
                schema: "dbo",
                table: "CompanyDetails",
                column: "Fk_BranchId",
                principalSchema: "dbo",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDetails_Branches_Fk_BranchId",
                schema: "dbo",
                table: "CompanyDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyDetails_Fk_BranchId",
                schema: "dbo",
                table: "CompanyDetails");

            migrationBuilder.DropColumn(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "CompanyDetails");

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                schema: "dbo",
                table: "CompanyDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
