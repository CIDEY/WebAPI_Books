using Microsoft.EntityFrameworkCore;
using BooksAPI.Model;

namespace BooksAPI.DBContextAPI
{
    public class BooksApiDb : DbContext
    {
        public BooksApiDb(DbContextOptions options) : base(options) { }

        public DbSet<Books> Books { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
