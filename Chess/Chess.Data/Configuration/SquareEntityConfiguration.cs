namespace Chess.Data.Configuration;

using Chess.Common;
using Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
