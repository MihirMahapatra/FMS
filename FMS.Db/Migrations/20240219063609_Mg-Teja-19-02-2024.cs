using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class MgTeja19022024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "dbo",
                table: "SalesTransaction",
                newName: "UnitQuantity");

            migrationBuilder.AddColumn<decimal>(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "SalesTransaction",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesTransaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "UnitQuantity",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransaction_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesTransaction",
                column: "Fk_AlternateUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnTransactions_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                column: "Fk_AlternateUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturnTransactions_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                column: "Fk_AlternateUnitId",
                principalSchema: "dbo",
                principalTable: "AlternateUnits",
                principalColumn: "AlternateUnitId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransaction_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesTransaction",
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
                name: "FK_SalesReturnTransactions_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransaction_AlternateUnits_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransaction_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturnTransactions_Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropColumn(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesTransaction");

            migrationBuilder.DropColumn(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropColumn(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropColumn(
                name: "UnitQuantity",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.RenameColumn(
                name: "UnitQuantity",
                schema: "dbo",
                table: "SalesTransaction",
                newName: "Quantity");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
