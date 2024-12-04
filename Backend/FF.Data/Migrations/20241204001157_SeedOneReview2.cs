using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOneReview2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7921));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7936));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7934));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7938));

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "CreatedAt", "IsReported", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[] { 5, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7940), null, 99, null, 0, 4.5, 1, "2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 0, 38, 626, DateTimeKind.Utc).AddTicks(4776));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 0, 38, 626, DateTimeKind.Utc).AddTicks(4784));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 0, 38, 626, DateTimeKind.Utc).AddTicks(4782));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 0, 38, 626, DateTimeKind.Utc).AddTicks(4785));
        }
    }
}
