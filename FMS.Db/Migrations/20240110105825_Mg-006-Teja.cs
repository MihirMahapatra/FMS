using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg006Teja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OTAmount",
                schema: "dbo",
                table: "ProductionEntries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTAmount",
                schema: "dbo",
                table: "ProductionEntries");
        }
    }
}
