namespace Chess.Data.Configuration;

using Chess.Common;
using Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BoardFiguresEntityConfiguration : IEntityTypeConfiguration<BoardFigures>
{
    public void Configure(EntityTypeBuilder<BoardFigures> builder)
    {
        builder
            .HasOne(x => x.Board)
            .WithMany()
            .HasForeignKey(x => x.BoardId);
    }
}
