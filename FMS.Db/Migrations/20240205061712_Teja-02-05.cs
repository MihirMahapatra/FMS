using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja0205 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesConfigs",
                schema: "dbo",
                columns: table => new
                {
                    SalesConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Fk_FinishedGoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fk_SubFinishedGoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesConfigs", x => x.SalesConfigId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesConfigs",
                schema: "dbo");
        }
    }
}
