using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mgteja0802 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Branches_Fk_BranchId",
                schema: "dbo",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Branches_Fk_BranchId",
                schema: "dbo",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_Fk_BranchId",
                schema: "dbo",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Fk_BranchId",
                schema: "dbo",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "States");

            migrationBuilder.DropColumn(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "Cities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "States",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_States_Fk_BranchId",
                schema: "dbo",
                table: "States",
                column: "Fk_BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Fk_BranchId",
                schema: "dbo",
                table: "Cities",
                column: "Fk_BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Branches_Fk_BranchId",
                schema: "dbo",
                table: "Cities",
                column: "Fk_BranchId",
                principalSchema: "dbo",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_States_Branches_Fk_BranchId",
                schema: "dbo",
                table: "States",
                column: "Fk_BranchId",
                principalSchema: "dbo",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
