using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class MG001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "SubLedgers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgers_Fk_BranchId",
                schema: "dbo",
                table: "SubLedgers",
                column: "Fk_BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubLedgers_Branches_Fk_BranchId",
                schema: "dbo",
                table: "SubLedgers",
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
                name: "FK_SubLedgers_Branches_Fk_BranchId",
                schema: "dbo",
                table: "SubLedgers");

            migrationBuilder.DropIndex(
                name: "IX_SubLedgers_Fk_BranchId",
                schema: "dbo",
                table: "SubLedgers");

            migrationBuilder.DropColumn(
                name: "Fk_BranchId",
                schema: "dbo",
                table: "SubLedgers");
        }
    }
}
