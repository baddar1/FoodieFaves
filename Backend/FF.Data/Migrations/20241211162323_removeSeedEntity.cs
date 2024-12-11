using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeSeedEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

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
                table: "Users",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Orders_OrderId",
                table: "Reviews",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Orders_OrderId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AdminId", "ApplicationUserId", "Email", "ImgUrl", "Password", "PhoneNumber", "ReviewCount", "TopRateReview", "TotalLikes", "TotalPoints", "UserName" },
                values: new object[,]
                {
                    { "1", null, null, null, "YazeedNada@gmail.com", "Y!", "Yazeed12.", null, 0, null, 0, 0, "YazeedNada" },
                    { "2", null, null, null, "Mohammadbaddar@gmail.com", "M!", "Mohd12.", null, 0, null, 0, 0, "Mohammadbaddar" }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AdminId", "Comment", "CreatedAt", "IsReported", "Likes", "NotificationId", "Points", "Rating", "RestaurantId", "UserId" },
                values: new object[,]
                {
                    { 1, null, "Nashville Fried Chicken, so so perfect !!", new DateTime(2024, 12, 11, 18, 57, 10, 973, DateTimeKind.Local).AddTicks(6762), null, 100, null, 0, 4.7000000000000002, 1, "1" },
                    { 2, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 11, 18, 57, 10, 973, DateTimeKind.Local).AddTicks(6788), null, 100, null, 0, 4.5, 1, "2" },
                    { 3, null, "so Juciyy !!", new DateTime(2024, 12, 11, 18, 57, 10, 973, DateTimeKind.Local).AddTicks(6784), null, 100, null, 0, 4.0999999999999996, 3, "1" },
                    { 4, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 11, 18, 57, 10, 973, DateTimeKind.Local).AddTicks(6792), null, 105, null, 0, 4.5, 1, "2" },
                    { 5, null, "Nashville Fried Chicken, so perfect !!", new DateTime(2024, 12, 11, 18, 57, 10, 973, DateTimeKind.Local).AddTicks(6794), null, 99, null, 0, 4.5, 1, "2" }
                });
        }
    }
}
