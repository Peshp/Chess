namespace Chess.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Chess.Data.Models;
    using Chess.Data.Models.Enums;
    using Chess.Data.Seeding;
    using Chess.Data.Seeding.Chess;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ChessDbContext(DbContextOptions<ChessDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole, string>(options)
    {
        public DbSet<Figure> Figures { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<Square> Squares { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            EntityIndexesConfiguration.Configure(builder);
        }
    }
}
