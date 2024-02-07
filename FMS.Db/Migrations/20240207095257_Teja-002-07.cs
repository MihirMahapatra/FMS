using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja00207 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HasSubLedger",
                schema: "dbo",
                table: "ledgersDev",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("1ecff7d8-702b-4dcd-93c5-b95a67e36fc9"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("701c663e-dac3-4a39-8d2a-36eb68426b54"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("712d600b-dfd6-4704-9e32-317fe62499a9"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("75e1fe3d-047d-41ad-a138-f0bb5bbc8b1f"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("7f740148-ed36-48ad-b194-031bc717842c"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("80025398-c02f-4a1a-9db7-8a21f9efd9ef"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("9bfa6931-977f-4a3d-a582-da5f1f4ab773"),
                column: "HasSubLedger",
                value: "No");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("d982b189-3326-430d-acde-13c12bba7992"),
                column: "HasSubLedger",
                value: "Yes");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("f07a3165-d63b-4dae-a820-ec79d83363b1"),
                column: "HasSubLedger",
                value: "Yes");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ledgersDev",
                keyColumn: "LedgerId",
                keyValue: new Guid("fbf4a6c7-c33d-4ad0-b7a5-abb319cc1b93"),
                column: "HasSubLedger",
                value: "Yes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSubLedger",
                schema: "dbo",
                table: "ledgersDev");
        }
    }
}
