using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFigureImageToSquares : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FigureImage",
                table: "BoardSquares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FigureImage",
                table: "BoardSquares");
        }
    }
}
