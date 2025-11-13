namespace Chess.Data;

using Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ChessDbContext(DbContextOptions<ChessDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole, string>(options)
{

    public DbSet<Figure> Figures { get; set; }

    public DbSet<Board> Boards { get; set; }

    public DbSet<Square> Squares { get; set; }

    public DbSet<BoardFigures> BoardFigures { get; set; }

    public DbSet<BoardSquares> BoardSquares { get; set; }

    public DbSet<UserBoard> UserBoards { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
