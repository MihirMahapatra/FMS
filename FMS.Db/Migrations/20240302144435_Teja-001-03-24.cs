using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja0010324 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturnTransactions_Products_Fk_ProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturnTransactions_Fk_ProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_SubProductId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnTransactions_Fk_SubProductId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                column: "Fk_SubProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturnTransactions_Products_Fk_SubProductId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                column: "Fk_SubProductId",
                principalSchema: "dbo",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturnTransactions_Products_Fk_SubProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturnTransactions_Fk_SubProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.DropColumn(
                name: "Fk_SubProductId",
                schema: "dbo",
                table: "SalesReturnTransactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "Fk_AlternateUnitId",
                schema: "dbo",
                table: "SalesReturnTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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
        }
    }
}
