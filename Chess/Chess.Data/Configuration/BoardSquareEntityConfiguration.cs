namespace Chess.Data.Configuration;

using Chess.Common;
using Chess.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BoardSquareEntityConfiguration : IEntityTypeConfiguration<BoardSquares>
{
    public void Configure(EntityTypeBuilder<BoardSquares> builder)
    {
        builder
            .HasOne(x => x.Board)
            .WithMany()
            .HasForeignKey(x => x.BoardId);
    }
}
