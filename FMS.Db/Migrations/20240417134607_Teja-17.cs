using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChallanNo",
                schema: "dbo",
                table: "InwardSupplyOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleNo",
                schema: "dbo",
                table: "InwardSupplyOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallanNo",
                schema: "dbo",
                table: "InwardSupplyOrders");

            migrationBuilder.DropColumn(
                name: "VehicleNo",
                schema: "dbo",
                table: "InwardSupplyOrders");
        }
    }
}
