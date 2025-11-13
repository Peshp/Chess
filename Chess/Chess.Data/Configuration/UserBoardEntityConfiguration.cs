namespace Chess.Data.Configuration
{
    using Chess.Data.Models;
    using static Chess.Common.BoardEntityConstants;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserBoardEntityConfiguration : IEntityTypeConfiguration<UserBoard>
    {
        public void Configure(EntityTypeBuilder<UserBoard> builder)
        {
            builder
                .Property<string>(x => x.Image)
                .HasMaxLength(BoardImageMaxLength);

            builder
                .Property<string>(x => x.UserId)
                .IsRequired(false);

            builder
                .HasOne<ApplicationUser>(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired(false);
        }
    }
}
