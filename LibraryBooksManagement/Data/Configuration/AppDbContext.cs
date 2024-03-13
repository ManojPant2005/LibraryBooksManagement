using LibraryBooksManagement.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksManagement.Data.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; } = default!;
    }
}
