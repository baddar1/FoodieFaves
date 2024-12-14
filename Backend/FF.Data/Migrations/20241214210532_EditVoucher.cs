using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vouchers_Restaurants_RestaurantId",
                table: "vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_vouchers_Users_UserId",
                table: "vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vouchers",
                table: "vouchers");

            migrationBuilder.RenameTable(
                name: "vouchers",
                newName: "Vouchers");

            migrationBuilder.RenameIndex(
                name: "IX_vouchers_UserId",
                table: "Vouchers",
                newName: "IX_Vouchers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_vouchers_RestaurantId",
                table: "Vouchers",
                newName: "IX_Vouchers_RestaurantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Restaurants_RestaurantId",
                table: "Vouchers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Users_UserId",
                table: "Vouchers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Restaurants_RestaurantId",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Users_UserId",
                table: "Vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers");

            migrationBuilder.RenameTable(
                name: "Vouchers",
                newName: "vouchers");

            migrationBuilder.RenameIndex(
                name: "IX_Vouchers_UserId",
                table: "vouchers",
                newName: "IX_vouchers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vouchers_RestaurantId",
                table: "vouchers",
                newName: "IX_vouchers_RestaurantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vouchers",
                table: "vouchers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_vouchers_Restaurants_RestaurantId",
                table: "vouchers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vouchers_Users_UserId",
                table: "vouchers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
