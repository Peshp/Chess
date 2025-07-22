namespace Chess.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;
    using Seeding;

    public class FigureEntityConfiguration : IEntityTypeConfiguration<Figure>
    {
        private ISeeder<Figure> seeder = new FigureSeeder();

        public void Configure(EntityTypeBuilder<Figure> builder)
        {
            builder.HasData(seeder.SeedDatabase());
        }
    }
}
