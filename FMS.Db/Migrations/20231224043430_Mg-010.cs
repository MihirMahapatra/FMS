using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg010 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseReturnTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseTransactions_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseTransactions",
                column: "Fk_AlternateUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnTransactions_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseReturnTransactions",
                column: "Fk_AlternateUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnTransactions_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseReturnTransactions",
                column: "Fk_AlternateUnitId",
                principalSchema: "dbo",
                principalTable: "AlternateUnits",
                principalColumn: "AlternateUnitId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseTransactions_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseTransactions",
                column: "Fk_AlternateUnitId",
                principalSchema: "dbo",
                principalTable: "AlternateUnits",
                principalColumn: "AlternateUnitId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnTransactions_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseReturnTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseTransactions_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseTransactions_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturnTransactions_Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseReturnTransactions");

            migrationBuilder.DropColumn(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseTransactions");

            migrationBuilder.DropColumn(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "PurchaseReturnTransactions");
        }
    }
}
