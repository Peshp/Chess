namespace Chess.Data.Seeding.Chess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using global::Chess.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.Extensions.Configuration;

    public class BoardEntitySeeder : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            Board board = this.SeedBoard();

            builder.HasData(board);
        }

        private Board SeedBoard()
        {
            var board = new Board
            {
                Id = 1,
                Image = "board.png",
            };

            return board;
        }
    }
}
