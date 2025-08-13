using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 1,
                column: "PositionY",
                value: 12.5);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 12.5, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 25.0, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 37.5, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 50.0, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 62.5, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 75.0, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 87.5, 12.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 10,
                column: "PositionX",
                value: 12.5);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 11,
                column: "PositionX",
                value: 25.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 12,
                column: "PositionX",
                value: 37.5);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 13,
                column: "PositionX",
                value: 50.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 14,
                column: "PositionX",
                value: 62.5);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 15,
                column: "PositionX",
                value: 75.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 16,
                column: "PositionX",
                value: 87.5);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17,
                column: "PositionY",
                value: 75.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 12.5, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 25.0, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 37.5, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 50.0, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 62.5, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 75.0, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 87.5, 75.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 25,
                column: "PositionY",
                value: 87.5);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Image", "PositionX", "PositionY" },
                values: new object[] { "bN.png", 12.5, 87.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 25.0, 87.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 37.5, 87.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 50.0, 87.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 62.5, 87.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 75.0, 87.5 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 87.5, 87.5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 1,
                column: "PositionY",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7.0, 1.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 10,
                column: "PositionX",
                value: 1.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 11,
                column: "PositionX",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 12,
                column: "PositionX",
                value: 3.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 13,
                column: "PositionX",
                value: 4.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 14,
                column: "PositionX",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 15,
                column: "PositionX",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 16,
                column: "PositionX",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17,
                column: "PositionY",
                value: 6.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 25,
                column: "PositionY",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Image", "PositionX", "PositionY" },
                values: new object[] { "BN.png", 1.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7.0, 7.0 });
        }
    }
}
