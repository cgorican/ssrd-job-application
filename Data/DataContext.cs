using Microsoft.EntityFrameworkCore;
using SSRD.Models;

namespace SSRD.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        /*
        public DataContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "WarningsDB.db");
        }
        */

        public DbSet<Warning> Warnings { get; set; }
        public DbSet<Author> Authors { get; set; }

        //public string DbPath { get; }

        //public DataContext(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }
}
