using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditTotalPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 1, 5, 13, 212, DateTimeKind.Utc).AddTicks(1664));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 1, 5, 13, 212, DateTimeKind.Utc).AddTicks(1677));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 1, 5, 13, 212, DateTimeKind.Utc).AddTicks(1675));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 1, 5, 13, 212, DateTimeKind.Utc).AddTicks(1679));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 1, 5, 13, 212, DateTimeKind.Utc).AddTicks(1681));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                column: "TotalPoints",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "TotalPoints",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 0, 52, 27, 5, DateTimeKind.Utc).AddTicks(5912));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 0, 52, 27, 5, DateTimeKind.Utc).AddTicks(5921));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 0, 52, 27, 5, DateTimeKind.Utc).AddTicks(5919));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 0, 52, 27, 5, DateTimeKind.Utc).AddTicks(5923));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 5, 0, 52, 27, 5, DateTimeKind.Utc).AddTicks(5925));
        }
    }
}
