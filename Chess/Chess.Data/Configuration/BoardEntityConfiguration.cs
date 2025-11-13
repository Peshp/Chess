namespace Chess.Data.Configuration;

using Chess.Common;
using Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BoardEntityConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property<string>(x => x.Image)
            .IsRequired(true)
            .HasMaxLength(BoardEntityConstants.BoardImageMaxLength);
    }
}
