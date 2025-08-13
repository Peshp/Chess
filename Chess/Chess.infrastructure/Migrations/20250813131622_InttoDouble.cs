using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chess.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InttoDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PositionY",
                table: "Figures",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "PositionX",
                table: "Figures",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0.0, 1.0 });

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
                keyValue: 9,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7.0, 0.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0.0, 6.0 });

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
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0.0, 7.0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1.0, 7.0 });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PositionY",
                table: "Figures",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PositionX",
                table: "Figures",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7, 1 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7, 0 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7, 6 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 0, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 1, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 2, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 3, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 4, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 5, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 6, 7 });

            migrationBuilder.UpdateData(
                table: "Figures",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 7, 7 });
        }
    }
}
