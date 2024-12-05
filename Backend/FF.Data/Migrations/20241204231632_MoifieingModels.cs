using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoifieingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TopRateReview",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalLikes",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserId1" },
                values: new object[] { new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7762), null });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId1" },
                values: new object[] { new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7777), null });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UserId1" },
                values: new object[] { new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7775), null });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UserId1" },
                values: new object[] { new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7779), null });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UserId1" },
                values: new object[] { new DateTime(2024, 12, 4, 23, 16, 31, 535, DateTimeKind.Utc).AddTicks(7780), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ReviewCount", "TopRateReview", "TotalLikes" },
                values: new object[] { 0, 0.0, 0 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ReviewCount", "TopRateReview", "TotalLikes" },
                values: new object[] { 0, 0.0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId1",
                table: "Reviews",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TopRateReview",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalLikes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reviews");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7921));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7936));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7934));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7938));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 4, 0, 11, 56, 466, DateTimeKind.Utc).AddTicks(7940));
        }
    }
}
