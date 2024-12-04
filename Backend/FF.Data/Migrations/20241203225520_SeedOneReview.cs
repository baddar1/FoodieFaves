using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOneReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "CreatedAt", "IsReported", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[] { 3, null, "so Juciyy !!", new DateTime(2024, 12, 3, 22, 55, 19, 563, DateTimeKind.Utc).AddTicks(4900), null, 100, null, 0, 4.0999999999999996, 3, "1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

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
    }
}
