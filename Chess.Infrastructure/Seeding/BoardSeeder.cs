namespace Chess.Infrastructure.Seeding
{
    using System.Collections.Generic;

    using Domain.Entities;

    public class BoardSeeder 
    {
        public Board SeedDatabase()
        {
            Board board = new Board
            {
                Id = 1,
                Name = "Standard Chess Board",
                Image = "board.png",
            };

            return board;
        }
    }
}
