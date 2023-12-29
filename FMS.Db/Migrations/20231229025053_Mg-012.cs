using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PriceType",
                schema: "dbo",
                table: "SalesReturnOrders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PriceType",
                schema: "dbo",
                table: "SalesOrders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "WholeSalePrice",
                schema: "dbo",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceType",
                schema: "dbo",
                table: "SalesReturnOrders");

            migrationBuilder.DropColumn(
                name: "PriceType",
                schema: "dbo",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "WholeSalePrice",
                schema: "dbo",
                table: "Products");
        }
    }
}
