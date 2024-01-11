using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class MG014 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "dbo",
                table: "PurchaseTransactions",
                newName: "UnitQuantity");

            migrationBuilder.AddColumn<decimal>(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "PurchaseTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UnitName",
                schema: "dbo",
                table: "PurchaseTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternateQuantity",
                schema: "dbo",
                table: "PurchaseTransactions");

            migrationBuilder.DropColumn(
                name: "UnitName",
                schema: "dbo",
                table: "PurchaseTransactions");

            migrationBuilder.RenameColumn(
                name: "UnitQuantity",
                schema: "dbo",
                table: "PurchaseTransactions",
                newName: "Quantity");
        }
    }
}
