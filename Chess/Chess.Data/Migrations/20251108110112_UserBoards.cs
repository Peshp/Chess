using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserBoards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserBoards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBoards_ApplicationUserId",
                table: "UserBoards",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBoards_AspNetUsers_ApplicationUserId",
                table: "UserBoards",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBoards_AspNetUsers_ApplicationUserId",
                table: "UserBoards");

            migrationBuilder.DropIndex(
                name: "IX_UserBoards_ApplicationUserId",
                table: "UserBoards");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserBoards");
        }
    }
}
