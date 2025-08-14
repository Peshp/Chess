using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPawns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 18,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 19,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 20,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 21,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 22,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 23,
                column: "Image",
                value: "bP.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 24,
                column: "Image",
                value: "bP.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 18,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 19,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 20,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 21,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 22,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 23,
                column: "Image",
                value: "bW.png");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 24,
                column: "Image",
                value: "bW.png");
        }
    }
}
