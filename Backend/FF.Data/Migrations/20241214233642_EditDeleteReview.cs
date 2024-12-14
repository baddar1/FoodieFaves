using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditDeleteReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBloggers_Users_UserId",
                table: "FavoriteBloggers");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteRestaurants_Restaurants_RestaurantId",
                table: "FavoriteRestaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteRestaurants_Users_UserId",
                table: "FavoriteRestaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Orders_OrderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Restaurants_RestaurantId",
                table: "TopReviewForUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Users_UserId",
                table: "TopReviewForUsers");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReviewId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReviewId",
                table: "Orders",
                column: "ReviewId",
                unique: true,
                filter: "[ReviewId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBloggers_Users_UserId",
                table: "FavoriteBloggers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteRestaurants_Restaurants_RestaurantId",
                table: "FavoriteRestaurants",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteRestaurants_Users_UserId",
                table: "FavoriteRestaurants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Restaurants_RestaurantId",
                table: "TopReviewForUsers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Users_UserId",
                table: "TopReviewForUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBloggers_Users_UserId",
                table: "FavoriteBloggers");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteRestaurants_Restaurants_RestaurantId",
                table: "FavoriteRestaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteRestaurants_Users_UserId",
                table: "FavoriteRestaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Restaurants_RestaurantId",
                table: "TopReviewForUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TopReviewForUsers_Users_UserId",
                table: "TopReviewForUsers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReviewId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderId",
                table: "Reviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReviewId",
                table: "Orders",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBloggers_Users_UserId",
                table: "FavoriteBloggers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteRestaurants_Restaurants_RestaurantId",
                table: "FavoriteRestaurants",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteRestaurants_Users_UserId",
                table: "FavoriteRestaurants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Orders_OrderId",
                table: "Reviews",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantId",
                table: "Reviews",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Restaurants_RestaurantId",
                table: "TopReviewForUsers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Reviews_ReviewId",
                table: "TopReviewForUsers",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopReviewForUsers_Users_UserId",
                table: "TopReviewForUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
