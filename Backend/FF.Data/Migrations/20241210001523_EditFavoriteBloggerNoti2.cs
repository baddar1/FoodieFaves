using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditFavoriteBloggerNoti2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Likes_LikeId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_LikeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LikeId",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 15, 22, 689, DateTimeKind.Local).AddTicks(675));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 15, 22, 689, DateTimeKind.Local).AddTicks(702));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 15, 22, 689, DateTimeKind.Local).AddTicks(697));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 15, 22, 689, DateTimeKind.Local).AddTicks(705));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 15, 22, 689, DateTimeKind.Local).AddTicks(709));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 7, 5, 998, DateTimeKind.Local).AddTicks(7402));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 7, 5, 998, DateTimeKind.Local).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 7, 5, 998, DateTimeKind.Local).AddTicks(7428));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 7, 5, 998, DateTimeKind.Local).AddTicks(7434));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 3, 7, 5, 998, DateTimeKind.Local).AddTicks(7436));

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_LikeId",
                table: "Notifications",
                column: "LikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Likes_LikeId",
                table: "Notifications",
                column: "LikeId",
                principalTable: "Likes",
                principalColumn: "Id");
        }
    }
}
