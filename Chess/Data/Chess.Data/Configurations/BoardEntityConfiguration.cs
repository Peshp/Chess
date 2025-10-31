using Chess.Common;
using Chess.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Data.Configurations
{
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
}
