using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOneRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AdminId", "Budget", "Cuisine", "Description", "Email", "ImgUrl", "Location", "Name", "Rating", "phoneNumber" },
                values: new object[] { 3, null, 4.0999999999999996, "shawerma", null, "", "Photo", "Jubiha", "saj", 0.0, "0799902599" });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 52, 36, 419, DateTimeKind.Utc).AddTicks(9293));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 52, 36, 419, DateTimeKind.Utc).AddTicks(9301));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 23, 17, 149, DateTimeKind.Utc).AddTicks(8307));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 3, 22, 23, 17, 149, DateTimeKind.Utc).AddTicks(8318));
        }
    }
}
