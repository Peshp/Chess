namespace Chess.Infrastructure.Seeding
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Enums;
    using Domain.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// Seeds initial chess figures (pieces) for both white and black sides into the database.
    /// Implements <see cref="ISeeder{Figure}"/> for use in database seeding.
    /// </summary>
    public class FigureSeeder : ISeeder<Figure>
    {
        public ICollection<Figure> SeedDatabase()
        {
            List<Figure> figures = new List<Figure>();

            foreach (var figure in CreateFigures("White", 6, 7, 100, 1, 2, 1))
                figures.Add(figure);

            foreach (var figure in CreateFigures("Black", 1, 0, 200, 9, 7, 8))
                figures.Add(figure);

            return figures;
        }

        /// <summary>
        /// Generates and adds chess figures for a specified color and starting positions to the provided collection.
        /// Creates pawns and major/minor pieces using standard chess setup parameters.
        /// Returns the updated collection containing all generated figures.
        /// </summary>
        private static ICollection<Figure> CreateFigures(string color, int pawnRow, int backRow, int pawnIdStart, int backIdStart, int pawnRank, int backRank)
        {
            List<Figure> figures = new List<Figure>();

            var pieceTypes = new[] { FigureType.Rook, FigureType.Knight, FigureType.Bishop, FigureType.Queen, FigureType.King, FigureType.Bishop, FigureType.Knight, FigureType.Rook };
            var pieceImages = new[] { "R", "N", "B", "Q", "K", "B", "N", "R" };

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
