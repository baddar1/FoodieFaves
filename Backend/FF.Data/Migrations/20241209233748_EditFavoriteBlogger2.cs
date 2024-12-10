using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditFavoriteBlogger2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_FavoriteBloggers_FavoriteBloggerUserId_FavoriteBloggerBloggerId",
                table: "TopReviewForUsers");

            migrationBuilder.DropIndex(
                name: "IX_TopReviewForUsers_FavoriteBloggerUserId_FavoriteBloggerBloggerId",
                table: "TopReviewForUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteBloggerBloggerId",
                table: "TopReviewForUsers");

            migrationBuilder.DropColumn(
                name: "FavoriteBloggerUserId",
                table: "TopReviewForUsers");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 37, 47, 180, DateTimeKind.Local).AddTicks(8577));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 37, 47, 180, DateTimeKind.Local).AddTicks(8623));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 37, 47, 180, DateTimeKind.Local).AddTicks(8619));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 37, 47, 180, DateTimeKind.Local).AddTicks(8626));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 37, 47, 180, DateTimeKind.Local).AddTicks(8629));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteBloggerBloggerId",
                table: "TopReviewForUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavoriteBloggerUserId",
                table: "TopReviewForUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 23, 17, 632, DateTimeKind.Local).AddTicks(1968));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 23, 17, 632, DateTimeKind.Local).AddTicks(1993));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 23, 17, 632, DateTimeKind.Local).AddTicks(1990));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 23, 17, 632, DateTimeKind.Local).AddTicks(1996));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 2, 23, 17, 632, DateTimeKind.Local).AddTicks(1999));

            migrationBuilder.CreateIndex(
                name: "IX_TopReviewForUsers_FavoriteBloggerUserId_FavoriteBloggerBloggerId",
                table: "TopReviewForUsers",
                columns: new[] { "FavoriteBloggerUserId", "FavoriteBloggerBloggerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_FavoriteBloggers_FavoriteBloggerUserId_FavoriteBloggerBloggerId",
                table: "TopReviewForUsers",
                columns: new[] { "FavoriteBloggerUserId", "FavoriteBloggerBloggerId" },
                principalTable: "FavoriteBloggers",
                principalColumns: new[] { "UserId", "BloggerId" });
        }
    }
}
