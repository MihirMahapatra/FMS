using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Teja06042024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Fk_PartyGroupId",
                schema: "dbo",
                table: "Parties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Fk_RefarenceId",
                schema: "dbo",
                table: "Parties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                schema: "dbo",
                table: "OutwardSupplyTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChallanNo",
                schema: "dbo",
                table: "OutwardSupplyOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleNo",
                schema: "dbo",
                table: "OutwardSupplyOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PartyGruops",
                schema: "dbo",
                columns: table => new
                {
                    PartyGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PartyGroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyGruops", x => x.PartyGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Referances",
                schema: "dbo",
                columns: table => new
                {
                    RefaranceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ReferanceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referances", x => x.RefaranceId);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PartyGruops",
                columns: new[] { "PartyGroupId", "PartyGroupName" },
                values: new object[,]
                {
                    { new Guid("6e23176b-af17-4c7b-b2b2-437b284d80df"), "Door Frame" },
                    { new Guid("a9873f41-79b2-4ec7-ae94-82bc12d2dbb7"), "General" },
                    { new Guid("c9e8d76f-3d1b-4a97-bab1-f99e56c672ff"), "WH" },
                    { new Guid("f38ea62e-d4a8-4d0c-9c3d-9e33f8b9f4e5"), "Water Tank" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Fk_PartyGroupId",
                schema: "dbo",
                table: "Parties",
                column: "Fk_PartyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Fk_RefarenceId",
                schema: "dbo",
                table: "Parties",
                column: "Fk_RefarenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OutwardSupplyTransactions_UnitId",
                schema: "dbo",
                table: "OutwardSupplyTransactions",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutwardSupplyTransactions_Units_UnitId",
                schema: "dbo",
                table: "OutwardSupplyTransactions",
                column: "UnitId",
                principalSchema: "dbo",
                principalTable: "Units",
                principalColumn: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_PartyGruops_Fk_PartyGroupId",
                schema: "dbo",
                table: "Parties",
                column: "Fk_PartyGroupId",
                principalSchema: "dbo",
                principalTable: "PartyGruops",
                principalColumn: "PartyGroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parties_Referances_Fk_RefarenceId",
                schema: "dbo",
                table: "Parties",
                column: "Fk_RefarenceId",
                principalSchema: "dbo",
                principalTable: "Referances",
                principalColumn: "RefaranceId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutwardSupplyTransactions_Units_UnitId",
                schema: "dbo",
                table: "OutwardSupplyTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_PartyGruops_Fk_PartyGroupId",
                schema: "dbo",
                table: "Parties");

            migrationBuilder.DropForeignKey(
                name: "FK_Parties_Referances_Fk_RefarenceId",
                schema: "dbo",
                table: "Parties");

            migrationBuilder.DropTable(
                name: "PartyGruops",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Referances",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Parties_Fk_PartyGroupId",
                schema: "dbo",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_Parties_Fk_RefarenceId",
                schema: "dbo",
                table: "Parties");

            migrationBuilder.DropIndex(
                name: "IX_OutwardSupplyTransactions_UnitId",
                schema: "dbo",
                table: "OutwardSupplyTransactions");

            migrationBuilder.DropColumn(
                name: "Fk_PartyGroupId",
                schema: "dbo",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "Fk_RefarenceId",
                schema: "dbo",
                table: "Parties");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "dbo",
                table: "OutwardSupplyTransactions");

            migrationBuilder.DropColumn(
                name: "ChallanNo",
                schema: "dbo",
                table: "OutwardSupplyOrders");

            migrationBuilder.DropColumn(
                name: "VehicleNo",
                schema: "dbo",
                table: "OutwardSupplyOrders");
        }
    }
}
