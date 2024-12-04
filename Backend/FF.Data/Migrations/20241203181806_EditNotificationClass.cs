using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditNotificationClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Notifications",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Notifications");
        }
    }
}
