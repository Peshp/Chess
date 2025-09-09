namespace Chess.Data.Seeding.Chess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using global::Chess.Data.Models;

    public class BoardEntitySeeder : IEntitySeeder
    {
        public async Task Seed(ChessDbContext dbContext)
        {
            Board board = this.SeedBoard();

            await dbContext.AddRangeAsync(board);
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
