namespace Chess.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using infrastructure.Entities;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Figure> Figures { get; set; } = null!;

        public DbSet<Board> Boards { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);               

            List<Figure> figures = new List<Figure>()
            {
                new Figure { Id = 1, Name = "Pawn", Color = "White", Image = "wP.png", PositionX = 0, PositionY = 1 },
                new Figure { Id = 2, Name = "Knight", Color = "White", Image = "wN.png", PositionX = 1, PositionY = 0 },
                new Figure { Id = 3, Name = "Bishop", Color = "White", Image = "wB.png", PositionX = 2, PositionY = 0 },
                new Figure { Id = 4, Name = "Rook", Color = "White", Image = "wR.png", PositionX = 0, PositionY = 0 },
                new Figure { Id = 5, Name = "Queen", Color = "White", Image = "wQ.png", PositionX = 3, PositionY = 0 },
                new Figure { Id = 6, Name = "King", Color = "White", Image = "wK.png", PositionX = 4, PositionY = 0 },

                new Figure { Id = 7, Name = "Pawn", Color = "Black", Image = "bP.png", PositionX = 0, PositionY = 1 },
                new Figure { Id = 8, Name = "Knight", Color = "Black", Image = "bN.png", PositionX = 1, PositionY = 0 },
                new Figure { Id = 9, Name = "Bishop", Color = "Black", Image = "bB.png", PositionX = 2, PositionY = 0 },
                new Figure { Id = 10, Name = "Rook", Color = "Black", Image = "bR.png", PositionX = 0, PositionY = 0 },
                new Figure { Id = 11, Name = "Queen", Color = "Black", Image = "bQ.png", PositionX = 3, PositionY = 0 },
                new Figure { Id = 12, Name = "King", Color = "Black", Image = "bK.png", PositionX = 4, PositionY = 0 },
            };

            foreach (var figure in figures)
            {
                builder.Entity<Figure>().HasData(figure);
            }

            Board board = new Board
            {
                Id = 1,
                Image = "board.png",
                Figures = figures,
            };
        }
    }
}
