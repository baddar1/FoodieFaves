using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOneReview1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "CreatedAt", "IsReported", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[] { 4, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 4, 0, 0, 38, 626, DateTimeKind.Utc).AddTicks(4785), null, 105, null, 0, 4.5, 1, "2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

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
    }
}
