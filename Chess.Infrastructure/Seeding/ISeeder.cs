namespace Chess.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>  
    /// Seeds the database with initial data asynchronously.  
    /// </summary>  
    /// <param name="DbContext">The database context to seed.</param>  
    public interface ISeeder<T> where T : class
    {
        Task SeedDatabaseAsync(ApplicationDbContext dbContext);
    }
}
