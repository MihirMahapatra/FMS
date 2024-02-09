using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class TejaMg0902 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RetailPrice",
                schema: "dbo",
                table: "AlternateUnits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WholeSalePrice",
                schema: "dbo",
                table: "AlternateUnits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetailPrice",
                schema: "dbo",
                table: "AlternateUnits");

            migrationBuilder.DropColumn(
                name: "WholeSalePrice",
                schema: "dbo",
                table: "AlternateUnits");
        }
    }
}
