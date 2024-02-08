using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mgteja080223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Narration",
                schema: "dbo",
                table: "LabourOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Narration",
                schema: "dbo",
                table: "LabourOrders");
        }
    }
}
