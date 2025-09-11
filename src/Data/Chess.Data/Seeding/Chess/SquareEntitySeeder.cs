namespace Chess.Data.Seeding.Chess
{
    using System.Collections.Generic;

    using global::Chess.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SquareEntitySeeder : IEntityTypeConfiguration<Square>
    {
        public void Configure(EntityTypeBuilder<Square> builder)
        {
            builder.HasData(this.SeedSquares());
        }

        private List<Square> SeedSquares()
        {
            var files = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            var squares = new List<Square>();
            int id = 1;

            for (int rank = 8; rank >= 1; rank--)
            {
                double positionY = (8 - rank) * 12.5;

                for (int file = 0; file < files.Length; file++)
                {
                    double positionX = file * 12.5;
                    string coordinate = $"{files[file]}{rank}";

                    squares.Add(
                        new Square
                        {
                            Id = id++,
                            PositionX = positionX,
                            PositionY = positionY,
                            Coordinate = coordinate
                        }
                    );
                }
            }

            return squares;
        }
    }
}
