using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja15032024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteAdress",
                schema: "dbo",
                table: "SalesReturnOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportationCharges",
                schema: "dbo",
                table: "SalesReturnOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HandlingCharges",
                schema: "dbo",
                table: "SalesOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SiteAdress",
                schema: "dbo",
                table: "SalesOrders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportationCharges",
                schema: "dbo",
                table: "SalesOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteAdress",
                schema: "dbo",
                table: "SalesReturnOrders");

            migrationBuilder.DropColumn(
                name: "TransportationCharges",
                schema: "dbo",
                table: "SalesReturnOrders");

            migrationBuilder.DropColumn(
                name: "HandlingCharges",
                schema: "dbo",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "SiteAdress",
                schema: "dbo",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "TransportationCharges",
                schema: "dbo",
                table: "SalesOrders");
        }
    }
}
