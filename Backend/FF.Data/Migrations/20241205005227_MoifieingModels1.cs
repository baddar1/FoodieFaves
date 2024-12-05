using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoifieingModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllPoints",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllPoints",
                table: "Points");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7762));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7777));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7779));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7780));
        }
    }
}
