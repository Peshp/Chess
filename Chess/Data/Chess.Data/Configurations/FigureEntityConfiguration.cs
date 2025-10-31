namespace Chess.Data.Configurations
{
    using Chess.Common;
    using Chess.Data.Models;
    using Chess.Data.Models.Enums;
    using Chess.Data.Seeding;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FigureEntityConfiguration : IEntityTypeConfiguration<Figure>
    {
        public void Configure(EntityTypeBuilder<Figure> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property<FigureType>(x => x.Type)
                .IsRequired(true);

            builder
                .Property<FigureColor>(x => x.Color)
                .IsRequired(true);

            builder
                .Property<string>(x => x.Image)
                .IsRequired(true)
                .HasMaxLength(FigureEntityConstants.FigureImageMaxLength);

            builder
                .HasOne(x => x.Board)
                .WithMany(x => x.Figures)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
        }
    }
}
