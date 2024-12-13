using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditTopReviewModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "TopReviewForUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 43, 7, 647, DateTimeKind.Local).AddTicks(2139));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 43, 7, 647, DateTimeKind.Local).AddTicks(2163));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 43, 7, 647, DateTimeKind.Local).AddTicks(2159));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 43, 7, 647, DateTimeKind.Local).AddTicks(2166));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 43, 7, 647, DateTimeKind.Local).AddTicks(2169));

            migrationBuilder.CreateIndex(
                name: "IX_TopReviewForUsers_RestaurantId",
                table: "TopReviewForUsers",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Restaurants_RestaurantId",
                table: "TopReviewForUsers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Restaurants_RestaurantId",
                table: "TopReviewForUsers");

            migrationBuilder.DropIndex(
                name: "IX_TopReviewForUsers_RestaurantId",
                table: "TopReviewForUsers");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "TopReviewForUsers");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 33, 41, 935, DateTimeKind.Local).AddTicks(9243));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 33, 41, 935, DateTimeKind.Local).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 33, 41, 935, DateTimeKind.Local).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 33, 41, 935, DateTimeKind.Local).AddTicks(9281));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 9, 22, 33, 41, 935, DateTimeKind.Local).AddTicks(9288));
        }
    }
}
