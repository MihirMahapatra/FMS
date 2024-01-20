using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FMS.Db.Migrations
{
    /// <inheritdoc />
    public partial class Mg03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LabourTypes",
                columns: new[] { "LabourTypeId", "Labour_Type" },
                values: new object[,]
                {
                    { new Guid("5e514855-55a0-459c-8b8b-def7696d9ad0"), "PRODUCTION" },
                    { new Guid("6c2758a2-79b5-43a6-8851-c6f975433b0f"), "SERVICE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LabourTypes",
                keyColumn: "LabourTypeId",
                keyValue: new Guid("5e514855-55a0-459c-8b8b-def7696d9ad0"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LabourTypes",
                keyColumn: "LabourTypeId",
                keyValue: new Guid("6c2758a2-79b5-43a6-8851-c6f975433b0f"));
        }
    }
}
