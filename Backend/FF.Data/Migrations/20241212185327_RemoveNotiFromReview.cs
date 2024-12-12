using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotiFromReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Notifications_NotificationId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_NotificationId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_NotificationId",
                table: "Reviews",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Notifications_NotificationId",
                table: "Reviews",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }
    }
}
