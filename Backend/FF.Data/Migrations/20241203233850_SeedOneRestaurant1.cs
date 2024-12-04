using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOneRestaurant1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AdminId", "Budget", "Cuisine", "Description", "Email", "ImgUrl", "Location", "Name", "Rating", "phoneNumber" },
                values: new object[] { 4, null, 2.0, "shawerma", null, "", "Photo", "Jubiha", "Reem", 0.0, "0799902599" });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 23, 38, 49, 549, DateTimeKind.Utc).AddTicks(623));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 23, 38, 49, 549, DateTimeKind.Utc).AddTicks(636));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 23, 38, 49, 549, DateTimeKind.Utc).AddTicks(634));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 55, 19, 563, DateTimeKind.Utc).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 55, 19, 563, DateTimeKind.Utc).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 55, 19, 563, DateTimeKind.Utc).AddTicks(4900));
        }
    }
}
