using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Restaurants_RestaurantId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Restaurants_RestaurantId",
                table: "Points",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Restaurants_RestaurantId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Restaurants_RestaurantId",
                table: "Points",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
