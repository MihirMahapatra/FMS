using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja0424 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SalesTransaction_Fk_ProductId",
                schema: "dbo",
                table: "SalesTransaction",
                column: "Fk_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnTransactions_Fk_ProductId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                column: "Fk_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturnTransactions_Products_Fk_ProductId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                column: "Fk_ProductId",
                principalSchema: "dbo",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransaction_Products_Fk_ProductId",
                schema: "dbo",
                table: "SalesTransaction",
                column: "Fk_ProductId",
                principalSchema: "dbo",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturnTransactions_Products_Fk_ProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransaction_Products_Fk_ProductId",
                schema: "dbo",
                table: "SalesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransaction_Fk_ProductId",
                schema: "dbo",
                table: "SalesTransaction");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturnTransactions_Fk_ProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");
        }
    }
}
