namespace Chess.Data.Seeding.Chess
{
    using System.Threading.Tasks;

    public interface IEntitySeeder
    {
        Task Seed(ChessDbContext dbContext);
    }
}
