using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTopReviewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalLikes",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AllPoints",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TopReviewForUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    TopRateReview = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopReviewForUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopReviewForUsers_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopReviewForUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AdminId", "Budget", "Cuisine", "Description", "Email", "ImgUrl", "Location", "Name", "Rating", "phoneNumber" },
                values: new object[,]
                {
                    { 3, null, 4.0999999999999996, "shawerma", null, "", "Photo", "Jubiha", "saj", 0.0, "0799902599" },
                    { 4, null, 2.0, "shawerma", null, "", "Photo", "Jubiha", "Reem", 0.0, "0799902599" }
                });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 6, 13, 944, DateTimeKind.Local).AddTicks(5955));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 6, 13, 944, DateTimeKind.Local).AddTicks(5981));

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "CreatedAt", "IsReported", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[,]
                {
                    { 4, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 9, 22, 6, 13, 944, DateTimeKind.Local).AddTicks(5984), null, 105, null, 0, 4.5, 1, "2" },
                    { 5, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 9, 22, 6, 13, 944, DateTimeKind.Local).AddTicks(5987), null, 99, null, 0, 4.5, 1, "2" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ReviewCount", "TotalLikes", "TotalPoints" },
                values: new object[] { 0, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ReviewCount", "TotalLikes", "TotalPoints" },
                values: new object[] { 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "CreatedAt", "IsReported", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[] { 3, null, "so Juciyy !!", new DateTime(2024, 12, 9, 22, 6, 13, 944, DateTimeKind.Local).AddTicks(5977), null, 100, null, 0, 4.0999999999999996, 3, "1" });

            migrationBuilder.CreateIndex(
                name: "IX_TopReviewForUsers_ReviewId",
                table: "TopReviewForUsers",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TopReviewForUsers_UserId",
                table: "TopReviewForUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopReviewForUsers");

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalLikes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AllPoints",
                table: "Points");
        }
    }
}
