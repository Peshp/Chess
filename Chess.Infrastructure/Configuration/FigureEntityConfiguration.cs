namespace Chess.Infrastructure.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    public class FigureEntityConfiguration : IEntityTypeConfiguration<Figure>
    {
        public void Configure(EntityTypeBuilder<Figure> builder)
        {
        }
    }
}
