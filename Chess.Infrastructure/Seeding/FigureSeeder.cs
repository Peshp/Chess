namespace Chess.Infrastructure.Seeding
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Enums;
    using Domain.Entities;
    using System;
    using System.Collections.Generic;

    public class FigureSeeder : ISeeder<Figure>
    {
        public void SeedDatabase()
        {
            ICollection<Figure> figures = new HashSet<Figure>();

            foreach (var figure in CreateFigures("White", 6, 7, 100, 1, 2, 1, figures))
                figures.Add(figure);

            foreach (var figure in CreateFigures("Black", 1, 0, 200, 9, 7, 8, figures))
                figures.Add(figure);
        }

        private static ICollection<Figure> CreateFigures(string color, int pawnRow, int backRow, int pawnIdStart, int backIdStart, int pawnRank, int backRank, ICollection<Figure> figures)
        {
            // Piece order and images for the back row
            var pieceTypes = new[] { FigureType.Rook, FigureType.Knight, FigureType.Bishop, FigureType.Queen, FigureType.King, FigureType.Bishop, FigureType.Knight, FigureType.Rook };
            var pieceImages = new[] { "R", "N", "B", "Q", "K", "B", "N", "R" };

            // Pawns
            for (int col = 0; col < 8; col++)
            {
                figures.Add(new Figure
                {
                    Id = pawnIdStart + col,
                    Row = pawnRow,
                    Col = col,
                    Type = FigureType.Pawn,
                    FigureImage = $"{color[0].ToString().ToLower()}P.png",
                    CurrentPosition = $"{(char)('A' + col)}{pawnRank}",
                    Color = color
                });
            }

            // Back row
            for (int col = 0; col < 8; col++)
            {
                figures.Add(new Figure
                {
                    Id = backIdStart + col,
                    Row = backRow,
                    Col = col,
                    Type = pieceTypes[col],
                    FigureImage = $"{color[0].ToString().ToLower()}{pieceImages[col]}.png",
                    CurrentPosition = $"{(char)('A' + col)}{backRank}",
                    Color = color,
                });
            }

            return figures;
        }

    }
}
