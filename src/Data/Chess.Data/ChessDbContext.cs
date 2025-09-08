namespace Chess.Data
{
    using Chess.Data.Models;
    using Chess.Data.Models.Enums;
    using Chess.Data.Seeding;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class ChessDbContext(DbContextOptions<ChessDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole, string>(options)
    {
        public DbSet<Figure> Figures { get; set; }

        public DbSet<Board> Boards { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            EntityIndexesConfiguration.Configure(builder);

            //var board = new Board
            //{
            //    Id = 1,
            //    Image = "board.png",
            //};

            //List<Figure> figures = new List<Figure>()
            //{
            //    // White major pieces
            //    new Figure { Id = 1,  BoardId = 1, Type = FigureType.Rook,   Color = FigureColor.White, Image = "wR.png", PositionX = 0.0,    PositionY = 87.5 },
            //    new Figure { Id = 2,  BoardId = 1, Type = FigureType.Knight, Color = FigureColor.White, Image = "wN.png", PositionX = 12.5,   PositionY = 87.5 },
            //    new Figure { Id = 3,  BoardId = 1, Type = FigureType.Bishop, Color = FigureColor.White, Image = "wB.png", PositionX = 25.0,   PositionY = 87.5 },
            //    new Figure { Id = 4,  BoardId = 1, Type = FigureType.Queen,  Color = FigureColor.White, Image = "wQ.png", PositionX = 37.5,   PositionY = 87.5 },
            //    new Figure { Id = 5,  BoardId = 1, Type = FigureType.King,   Color = FigureColor.White, Image = "wK.png", PositionX = 50.0,   PositionY = 87.5 },
            //    new Figure { Id = 6,  BoardId = 1, Type = FigureType.Bishop, Color = FigureColor.White, Image = "wB.png", PositionX = 62.5,   PositionY = 87.5 },
            //    new Figure { Id = 7,  BoardId = 1, Type = FigureType.Knight, Color = FigureColor.White, Image = "wN.png", PositionX = 75.0,   PositionY = 87.5 },
            //    new Figure { Id = 8,  BoardId = 1, Type = FigureType.Rook,   Color = FigureColor.White, Image = "wR.png", PositionX = 87.5,   PositionY = 87.5 },

            //    // White pawns
            //    new Figure { Id = 9,  BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 0.0,    PositionY = 75.0 },
            //    new Figure { Id = 10, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 12.5,   PositionY = 75.0 },
            //    new Figure { Id = 11, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 25.0,   PositionY = 75.0 },
            //    new Figure { Id = 12, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 37.5,   PositionY = 75.0 },
            //    new Figure { Id = 13, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 50.0,   PositionY = 75.0 },
            //    new Figure { Id = 14, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 62.5,   PositionY = 75.0 },
            //    new Figure { Id = 15, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 75.0,   PositionY = 75.0 },
            //    new Figure { Id = 16, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.White, Image = "wP.png", PositionX = 87.5,   PositionY = 75.0 },

            //    // Black pawns
            //    new Figure { Id = 17, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 0.0,    PositionY = 12.5 },
            //    new Figure { Id = 18, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 12.5,   PositionY = 12.5 },
            //    new Figure { Id = 19, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 25.0,   PositionY = 12.5 },
            //    new Figure { Id = 20, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 37.5,   PositionY = 12.5 },
            //    new Figure { Id = 21, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 50.0,   PositionY = 12.5 },
            //    new Figure { Id = 22, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 62.5,   PositionY = 12.5 },
            //    new Figure { Id = 23, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 75.0,   PositionY = 12.5 },
            //    new Figure { Id = 24, BoardId = 1, Type = FigureType.Pawn,   Color = FigureColor.Black, Image = "bP.png", PositionX = 87.5,   PositionY = 12.5 },

            //    // Black major pieces
            //    new Figure { Id = 25, BoardId = 1, Type = FigureType.Rook,   Color = FigureColor.Black, Image = "bR.png", PositionX = 0.0,    PositionY = 0.0 },
            //    new Figure { Id = 26, BoardId = 1, Type = FigureType.Knight, Color = FigureColor.Black, Image = "bN.png", PositionX = 12.5,   PositionY = 0.0 },
            //    new Figure { Id = 27, BoardId = 1, Type = FigureType.Bishop, Color = FigureColor.Black, Image = "bB.png", PositionX = 25.0,   PositionY = 0.0 },
            //    new Figure { Id = 28, BoardId = 1, Type = FigureType.Queen,  Color = FigureColor.Black, Image = "bQ.png", PositionX = 37.5,   PositionY = 0.0 },
            //    new Figure { Id = 29, BoardId = 1, Type = FigureType.King,   Color = FigureColor.Black, Image = "bK.png", PositionX = 50.0,   PositionY = 0.0 },
            //    new Figure { Id = 30, BoardId = 1, Type = FigureType.Bishop, Color = FigureColor.Black, Image = "bB.png", PositionX = 62.5,   PositionY = 0.0 },
            //    new Figure { Id = 31, BoardId = 1, Type = FigureType.Knight, Color = FigureColor.Black, Image = "bN.png", PositionX = 75.0,   PositionY = 0.0 },
            //    new Figure { Id = 32, BoardId = 1, Type = FigureType.Rook,   Color = FigureColor.Black, Image = "bR.png", PositionX = 87.5,   PositionY = 0.0 },
            //};

            //builder.Entity<Board>().HasData(board);
            //builder.Entity<Figure>().HasData(figures);        }
        }
    }
}
