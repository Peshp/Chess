using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chess.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Image" },
                values: new object[] { 1, "board.png" });

            migrationBuilder.InsertData(
                table: "Figures",
                columns: new[] { "Id", "BoardId", "Color", "Image", "PositionX", "PositionY", "Type" },
                values: new object[,]
                {
                    { 1, 1, 1, "wR.png", 0.0, 87.5, 4 },
                    { 2, 1, 1, "wN.png", 12.5, 87.5, 2 },
                    { 3, 1, 1, "wB.png", 25.0, 87.5, 0 },
                    { 4, 1, 1, "wQ.png", 37.5, 87.5, 5 },
                    { 5, 1, 1, "wK.png", 50.0, 87.5, 1 },
                    { 6, 1, 1, "wB.png", 62.5, 87.5, 0 },
                    { 7, 1, 1, "wN.png", 75.0, 87.5, 2 },
                    { 8, 1, 1, "wR.png", 87.5, 87.5, 4 },
                    { 9, 1, 1, "wP.png", 0.0, 75.0, 3 },
                    { 10, 1, 1, "wP.png", 12.5, 75.0, 3 },
                    { 11, 1, 1, "wP.png", 25.0, 75.0, 3 },
                    { 12, 1, 1, "wP.png", 37.5, 75.0, 3 },
                    { 13, 1, 1, "wP.png", 50.0, 75.0, 3 },
                    { 14, 1, 1, "wP.png", 62.5, 75.0, 3 },
                    { 15, 1, 1, "wP.png", 75.0, 75.0, 3 },
                    { 16, 1, 1, "wP.png", 87.5, 75.0, 3 },
                    { 17, 1, 0, "bP.png", 0.0, 12.5, 3 },
                    { 18, 1, 0, "bP.png", 12.5, 12.5, 3 },
                    { 19, 1, 0, "bP.png", 25.0, 12.5, 3 },
                    { 20, 1, 0, "bP.png", 37.5, 12.5, 3 },
                    { 21, 1, 0, "bP.png", 50.0, 12.5, 3 },
                    { 22, 1, 0, "bP.png", 62.5, 12.5, 3 },
                    { 23, 1, 0, "bP.png", 75.0, 12.5, 3 },
                    { 24, 1, 0, "bP.png", 87.5, 12.5, 3 },
                    { 25, 1, 0, "bR.png", 0.0, 0.0, 4 },
                    { 26, 1, 0, "bN.png", 12.5, 0.0, 2 },
                    { 27, 1, 0, "bB.png", 25.0, 0.0, 0 },
                    { 28, 1, 0, "bQ.png", 37.5, 0.0, 5 },
                    { 29, 1, 0, "bK.png", 50.0, 0.0, 1 },
                    { 30, 1, 0, "bB.png", 62.5, 0.0, 0 },
                    { 31, 1, 0, "bN.png", 75.0, 0.0, 2 },
                    { 32, 1, 0, "bR.png", 87.5, 0.0, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
