using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabaseFigureEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Figures",
                columns: new[] { "Id", "Col", "Color", "CurrentPosition", "FigureImage", "MoveHistory", "Row", "Type" },
                values: new object[,]
                {
                    { 1, 0, "White", "A1", "wR.png", "[]", 7, 5 },
                    { 2, 1, "White", "B1", "wN.png", "[]", 7, 1 },
                    { 3, 2, "White", "C1", "wB.png", "[]", 7, 2 },
                    { 4, 3, "White", "D1", "wQ.png", "[]", 7, 3 },
                    { 5, 4, "White", "E1", "wK.png", "[]", 7, 4 },
                    { 6, 5, "White", "F1", "wB.png", "[]", 7, 2 },
                    { 7, 6, "White", "G1", "wN.png", "[]", 7, 1 },
                    { 8, 7, "White", "H1", "wR.png", "[]", 7, 5 },
                    { 9, 0, "Black", "A8", "bR.png", "[]", 0, 5 },
                    { 10, 1, "Black", "B8", "bN.png", "[]", 0, 1 },
                    { 11, 2, "Black", "C8", "bB.png", "[]", 0, 2 },
                    { 12, 3, "Black", "D8", "bQ.png", "[]", 0, 3 },
                    { 13, 4, "Black", "E8", "bK.png", "[]", 0, 4 },
                    { 14, 5, "Black", "F8", "bB.png", "[]", 0, 2 },
                    { 15, 6, "Black", "G8", "bN.png", "[]", 0, 1 },
                    { 16, 7, "Black", "H8", "bR.png", "[]", 0, 5 },
                    { 100, 0, "White", "A2", "wP.png", "[]", 6, 0 },
                    { 101, 1, "White", "B2", "wP.png", "[]", 6, 0 },
                    { 102, 2, "White", "C2", "wP.png", "[]", 6, 0 },
                    { 103, 3, "White", "D2", "wP.png", "[]", 6, 0 },
                    { 104, 4, "White", "E2", "wP.png", "[]", 6, 0 },
                    { 105, 5, "White", "F2", "wP.png", "[]", 6, 0 },
                    { 106, 6, "White", "G2", "wP.png", "[]", 6, 0 },
                    { 107, 7, "White", "H2", "wP.png", "[]", 6, 0 },
                    { 200, 0, "Black", "A7", "bP.png", "[]", 1, 0 },
                    { 201, 1, "Black", "B7", "bP.png", "[]", 1, 0 },
                    { 202, 2, "Black", "C7", "bP.png", "[]", 1, 0 },
                    { 203, 3, "Black", "D7", "bP.png", "[]", 1, 0 },
                    { 204, 4, "Black", "E7", "bP.png", "[]", 1, 0 },
                    { 205, 5, "Black", "F7", "bP.png", "[]", 1, 0 },
                    { 206, 6, "Black", "G7", "bP.png", "[]", 1, 0 },
                    { 207, 7, "Black", "H7", "bP.png", "[]", 1, 0 }
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
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 207);
        }
    }
}
