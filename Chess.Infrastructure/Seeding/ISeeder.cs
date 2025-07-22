namespace Chess.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Domain.Entities;

    /// <summary>  
    /// Seeds the database with initial data asynchronously.  
    /// </summary>  
    /// <param name="DbContext">The database context to seed.</param>  
    public interface ISeeder<T> where T : class
    {
        ICollection<Figure> SeedDatabase();
    }
}
