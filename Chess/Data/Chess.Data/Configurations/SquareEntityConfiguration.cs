namespace Chess.Data.Configurations
{
    using Chess.Common;
    using Chess.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using static Chess.Common.SquareEntityConstants;

    public class SquareEntityConfiguration : IEntityTypeConfiguration<Square>
    {
        public void Configure(EntityTypeBuilder<Square> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Coordinate)
                .IsRequired(true)
                .HasMaxLength(SquareEntityConstants.SquareStringMaxLength);
        }
    }
}
