using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microservices._2PhaseCommit.Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class mg_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0bb5ae92-5749-4f53-83bd-66ce78e0f5b4"), "Stock.API" },
                    { new Guid("8b764654-2268-419a-9b37-37b5a84cc276"), "Payment.API" },
                    { new Guid("c1df9a1d-e204-4ad0-b96f-cbaa77ed0817"), "Order.API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("0bb5ae92-5749-4f53-83bd-66ce78e0f5b4"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("8b764654-2268-419a-9b37-37b5a84cc276"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("c1df9a1d-e204-4ad0-b96f-cbaa77ed0817"));
        }
    }
}
