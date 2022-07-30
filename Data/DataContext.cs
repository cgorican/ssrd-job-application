using Microsoft.EntityFrameworkCore;
using SSRD.Models;

namespace SSRD.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Warning> Warnings { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
