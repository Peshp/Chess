using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedBoardntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[] { 1, "board.png", "Standard Chess Board" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
