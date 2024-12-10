using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewCountToRes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReviewCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReviewCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReviewCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReviewCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 35, 10, 754, DateTimeKind.Local).AddTicks(4271));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 35, 10, 754, DateTimeKind.Local).AddTicks(4293));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 35, 10, 754, DateTimeKind.Local).AddTicks(4290));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 35, 10, 754, DateTimeKind.Local).AddTicks(4342));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 35, 10, 754, DateTimeKind.Local).AddTicks(4345));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Restaurants");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 31, 17, 449, DateTimeKind.Local).AddTicks(8795));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 31, 17, 449, DateTimeKind.Local).AddTicks(8821));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 31, 17, 449, DateTimeKind.Local).AddTicks(8818));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 31, 17, 449, DateTimeKind.Local).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 10, 14, 31, 17, 449, DateTimeKind.Local).AddTicks(8826));
        }
    }
}
