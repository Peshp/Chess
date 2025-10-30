using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chess.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Figures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Figures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Figures_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Squares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionX = table.Column<double>(type: "float", nullable: false),
                    PositionY = table.Column<double>(type: "float", nullable: false),
                    Coordinate = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Squares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Squares_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "Squares",
                columns: new[] { "Id", "BoardId", "Coordinate", "PositionX", "PositionY" },
                values: new object[,]
                {
                    { 1, 1, "a8", 0.0, 0.0 },
                    { 2, 1, "b8", 12.5, 0.0 },
                    { 3, 1, "c8", 25.0, 0.0 },
                    { 4, 1, "d8", 37.5, 0.0 },
                    { 5, 1, "e8", 50.0, 0.0 },
                    { 6, 1, "f8", 62.5, 0.0 },
                    { 7, 1, "g8", 75.0, 0.0 },
                    { 8, 1, "h8", 87.5, 0.0 },
                    { 9, 1, "a7", 0.0, 12.5 },
                    { 10, 1, "b7", 12.5, 12.5 },
                    { 11, 1, "c7", 25.0, 12.5 },
                    { 12, 1, "d7", 37.5, 12.5 },
                    { 13, 1, "e7", 50.0, 12.5 },
                    { 14, 1, "f7", 62.5, 12.5 },
                    { 15, 1, "g7", 75.0, 12.5 },
                    { 16, 1, "h7", 87.5, 12.5 },
                    { 17, 1, "a6", 0.0, 25.0 },
                    { 18, 1, "b6", 12.5, 25.0 },
                    { 19, 1, "c6", 25.0, 25.0 },
                    { 20, 1, "d6", 37.5, 25.0 },
                    { 21, 1, "e6", 50.0, 25.0 },
                    { 22, 1, "f6", 62.5, 25.0 },
                    { 23, 1, "g6", 75.0, 25.0 },
                    { 24, 1, "h6", 87.5, 25.0 },
                    { 25, 1, "a5", 0.0, 37.5 },
                    { 26, 1, "b5", 12.5, 37.5 },
                    { 27, 1, "c5", 25.0, 37.5 },
                    { 28, 1, "d5", 37.5, 37.5 },
                    { 29, 1, "e5", 50.0, 37.5 },
                    { 30, 1, "f5", 62.5, 37.5 },
                    { 31, 1, "g5", 75.0, 37.5 },
                    { 32, 1, "h5", 87.5, 37.5 },
                    { 33, 1, "a4", 0.0, 50.0 },
                    { 34, 1, "b4", 12.5, 50.0 },
                    { 35, 1, "c4", 25.0, 50.0 },
                    { 36, 1, "d4", 37.5, 50.0 },
                    { 37, 1, "e4", 50.0, 50.0 },
                    { 38, 1, "f4", 62.5, 50.0 },
                    { 39, 1, "g4", 75.0, 50.0 },
                    { 40, 1, "h4", 87.5, 50.0 },
                    { 41, 1, "a3", 0.0, 62.5 },
                    { 42, 1, "b3", 12.5, 62.5 },
                    { 43, 1, "c3", 25.0, 62.5 },
                    { 44, 1, "d3", 37.5, 62.5 },
                    { 45, 1, "e3", 50.0, 62.5 },
                    { 46, 1, "f3", 62.5, 62.5 },
                    { 47, 1, "g3", 75.0, 62.5 },
                    { 48, 1, "h3", 87.5, 62.5 },
                    { 49, 1, "a2", 0.0, 75.0 },
                    { 50, 1, "b2", 12.5, 75.0 },
                    { 51, 1, "c2", 25.0, 75.0 },
                    { 52, 1, "d2", 37.5, 75.0 },
                    { 53, 1, "e2", 50.0, 75.0 },
                    { 54, 1, "f2", 62.5, 75.0 },
                    { 55, 1, "g2", 75.0, 75.0 },
                    { 56, 1, "h2", 87.5, 75.0 },
                    { 57, 1, "a1", 0.0, 87.5 },
                    { 58, 1, "b1", 12.5, 87.5 },
                    { 59, 1, "c1", 25.0, 87.5 },
                    { 60, 1, "d1", 37.5, 87.5 },
                    { 61, 1, "e1", 50.0, 87.5 },
                    { 62, 1, "f1", 62.5, 87.5 },
                    { 63, 1, "g1", 75.0, 87.5 },
                    { 64, 1, "h1", 87.5, 87.5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Figures_BoardId",
                table: "Figures",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Squares_BoardId",
                table: "Squares",
                column: "BoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Figures");

            migrationBuilder.DropTable(
                name: "Squares");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
