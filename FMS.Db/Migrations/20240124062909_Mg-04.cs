using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabourType",
                schema: "dbo",
                table: "LabourOrders");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "dbo",
                table: "PurchaseReturnTransactions",
                newName: "UnitQuantity");

            migrationBuilder.AddColumn<decimal>(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "PurchaseReturnTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_LabourTypeId",
                schema: "dbo",
                table: "LabourOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LabourOrders_Fk_LabourTypeId",
                schema: "dbo",
                table: "LabourOrders",
                column: "Fk_LabourTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabourOrders_LabourTypes_Fk_LabourTypeId",
                schema: "dbo",
                table: "LabourOrders",
                column: "Fk_LabourTypeId",
                principalSchema: "dbo",
                principalTable: "LabourTypes",
                principalColumn: "LabourTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabourOrders_LabourTypes_Fk_LabourTypeId",
                schema: "dbo",
                table: "LabourOrders");

            migrationBuilder.DropIndex(
                name: "IX_LabourOrders_Fk_LabourTypeId",
                schema: "dbo",
                table: "LabourOrders");

            migrationBuilder.DropColumn(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "PurchaseReturnTransactions");

            migrationBuilder.DropColumn(
                name: "Fk_LabourTypeId",
                schema: "dbo",
                table: "LabourOrders");

            migrationBuilder.RenameColumn(
                name: "UnitQuantity",
                schema: "dbo",
                table: "PurchaseReturnTransactions",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "LabourType",
                schema: "dbo",
                table: "LabourOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
