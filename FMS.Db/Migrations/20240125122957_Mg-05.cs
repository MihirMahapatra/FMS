using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseReturnOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnOrders_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseReturnOrders",
                column: "Fk_ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "Fk_ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "Fk_ProductTypeId",
                principalSchema: "dbo",
                principalTable: "ProductTypes",
                principalColumn: "ProductTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturnOrders_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseReturnOrders",
                column: "Fk_ProductTypeId",
                principalSchema: "dbo",
                principalTable: "ProductTypes",
                principalColumn: "ProductTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturnOrders_ProductTypes_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseReturnOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturnOrders_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseReturnOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseReturnOrders");

            migrationBuilder.DropColumn(
                name: "Fk_ProductTypeId",
                schema: "dbo",
                table: "PurchaseOrders");
        }
    }
}
