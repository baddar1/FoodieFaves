using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[,]
                {
                    { 2, null, "Nashville Fried Chicken, so perfect !!", 100, null, 5, 4.5, 1, "2" },
                    { 3, null, "saj, jucy Shawrmah perfect !!", 120, null, 5, 4.5, 2, "1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[,]
                {
                    { 4, null, "Nashville Fried Chicken, so perfect !!", 100, null, 5, 4.5, 1, "2" },
                    { 5, null, "saj, jucy Shawrmah perfect !!", 120, null, 5, 4.5, 2, "1" }
                });
        }
    }
}
