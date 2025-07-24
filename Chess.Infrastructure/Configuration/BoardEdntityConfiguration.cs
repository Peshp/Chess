namespace Chess.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Infrastructure.Seeding;

    public class BoardEdntityConfiguration : IEntityTypeConfiguration<Board>
    {
        private readonly BoardSeeder boardSeeder = new BoardSeeder();

        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder
                .HasData(boardSeeder.SeedDatabase());
        }
    }
}
