using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class asd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RestaurantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "FavoriteBloggers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBloggers_RestaurantId",
                table: "FavoriteBloggers",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBloggers_Restaurants_RestaurantId",
                table: "FavoriteBloggers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBloggers_Restaurants_RestaurantId",
                table: "FavoriteBloggers");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteBloggers_RestaurantId",
                table: "FavoriteBloggers");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "FavoriteBloggers");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "RestaurantId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RestaurantId",
                table: "Users",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
