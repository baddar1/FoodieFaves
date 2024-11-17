using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FF.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AdminId", "ApplicationUserId", "Email", "Password", "PhoneNumber", "Points", "UserId", "UserName" },
                values: new object[,]
                {
                    { "1", null, null, null, "YazeedNada@gmail.com", "Yazeed12.", null, 0, null, "YazeedNada" },
                    { "2", null, null, null, "Mohammadbaddar@gmail.com", "Mohd12.", null, 0, null, "Mohammadbaddar" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
