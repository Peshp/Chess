using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chess.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedSquareEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Square",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false),
                    Coordinate = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Square", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Square",
                columns: new[] { "Id", "Coordinate", "PositionX", "PositionY" },
                values: new object[,]
                {
                    { 1, "a8", 0.0, 0.0 },
                    { 2, "b8", 12.5, 0.0 },
                    { 3, "c8", 25.0, 0.0 },
                    { 4, "d8", 37.5, 0.0 },
                    { 5, "e8", 50.0, 0.0 },
                    { 6, "f8", 62.5, 0.0 },
                    { 7, "g8", 75.0, 0.0 },
                    { 8, "h8", 87.5, 0.0 },
                    { 9, "a7", 0.0, 12.5 },
                    { 10, "b7", 12.5, 12.5 },
                    { 11, "c7", 25.0, 12.5 },
                    { 12, "d7", 37.5, 12.5 },
                    { 13, "e7", 50.0, 12.5 },
                    { 14, "f7", 62.5, 12.5 },
                    { 15, "g7", 75.0, 12.5 },
                    { 16, "h7", 87.5, 12.5 },
                    { 17, "a6", 0.0, 25.0 },
                    { 18, "b6", 12.5, 25.0 },
                    { 19, "c6", 25.0, 25.0 },
                    { 20, "d6", 37.5, 25.0 },
                    { 21, "e6", 50.0, 25.0 },
                    { 22, "f6", 62.5, 25.0 },
                    { 23, "g6", 75.0, 25.0 },
                    { 24, "h6", 87.5, 25.0 },
                    { 25, "a5", 0.0, 37.5 },
                    { 26, "b5", 12.5, 37.5 },
                    { 27, "c5", 25.0, 37.5 },
                    { 28, "d5", 37.5, 37.5 },
                    { 29, "e5", 50.0, 37.5 },
                    { 30, "f5", 62.5, 37.5 },
                    { 31, "g5", 75.0, 37.5 },
                    { 32, "h5", 87.5, 37.5 },
                    { 33, "a4", 0.0, 50.0 },
                    { 34, "b4", 12.5, 50.0 },
                    { 35, "c4", 25.0, 50.0 },
                    { 36, "d4", 37.5, 50.0 },
                    { 37, "e4", 50.0, 50.0 },
                    { 38, "f4", 62.5, 50.0 },
                    { 39, "g4", 75.0, 50.0 },
                    { 40, "h4", 87.5, 50.0 },
                    { 41, "a3", 0.0, 62.5 },
                    { 42, "b3", 12.5, 62.5 },
                    { 43, "c3", 25.0, 62.5 },
                    { 44, "d3", 37.5, 62.5 },
                    { 45, "e3", 50.0, 62.5 },
                    { 46, "f3", 62.5, 62.5 },
                    { 47, "g3", 75.0, 62.5 },
                    { 48, "h3", 87.5, 62.5 },
                    { 49, "a2", 0.0, 75.0 },
                    { 50, "b2", 12.5, 75.0 },
                    { 51, "c2", 25.0, 75.0 },
                    { 52, "d2", 37.5, 75.0 },
                    { 53, "e2", 50.0, 75.0 },
                    { 54, "f2", 62.5, 75.0 },
                    { 55, "g2", 75.0, 75.0 },
                    { 56, "h2", 87.5, 75.0 },
                    { 57, "a1", 0.0, 87.5 },
                    { 58, "b1", 12.5, 87.5 },
                    { 59, "c1", 25.0, 87.5 },
                    { 60, "d1", 37.5, 87.5 },
                    { 61, "e1", 50.0, 87.5 },
                    { 62, "f1", 62.5, 87.5 },
                    { 63, "g1", 75.0, 87.5 },
                    { 64, "h1", 87.5, 87.5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Square");
        }
    }
}
