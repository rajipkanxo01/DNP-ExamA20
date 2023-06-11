using AuthorAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthorAPI.EFCDataAccess;

public class AuthorContext : DbContext
{
    public DbSet<Author>? Authors { get; set; }
    public DbSet<Book>? Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = Author.db");
    }
}