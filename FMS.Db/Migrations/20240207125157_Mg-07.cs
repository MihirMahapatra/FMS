using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                schema: "dbo",
                table: "PurchaseReturnOrders");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                schema: "dbo",
                table: "PurchaseReturnOrders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                schema: "dbo",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                schema: "dbo",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<decimal>(
                name: "TransportationCharges",
                schema: "dbo",
                table: "PurchaseReturnOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportationCharges",
                schema: "dbo",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ledgersDev",
                columns: new[] { "LedgerId", "Fk_LedgerGroupId", "Fk_LedgerSubGroupId", "HasSubLedger", "LedgerName", "LedgerType" },
                values: new object[,]
                {
                    { new Guid("9efd7830-125a-40e3-8f44-68ab03f52591"), new Guid("15fe2512-d922-45c5-9e03-64c32b903a5b"), null, "No", "Transporting Charges Recive", "None" },
                    { new Guid("d281cbfb-3cac-4c6a-8ce1-7b51973b8ca4"), new Guid("4458bce5-4546-4120-a7de-03acefd07b85"), null, "No", "Transporting Charges Payment", "None" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("9efd7830-125a-40e3-8f44-68ab03f52591"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("d281cbfb-3cac-4c6a-8ce1-7b51973b8ca4"));

            migrationBuilder.DropColumn(
                name: "TransportationCharges",
                schema: "dbo",
                table: "PurchaseReturnOrders");

            migrationBuilder.DropColumn(
                name: "TransportationCharges",
                schema: "dbo",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                schema: "dbo",
                table: "PurchaseReturnOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                schema: "dbo",
                table: "PurchaseReturnOrders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                schema: "dbo",
                table: "PurchaseOrders",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                schema: "dbo",
                table: "PurchaseOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
