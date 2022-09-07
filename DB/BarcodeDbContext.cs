using BarcodeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BarcodeAPI.DB
{
    public class BarcodeDbContext : DbContext
    {
        public string conn;
        public bool isDead = false;
        private DbContextOptionsBuilder _optionsBuilder;
        public BarcodeDbContext()
        {
            Database.EnsureCreated();
            Database.Migrate();
            //Database.ensu
        }

        public BarcodeDbContext(DbContextOptions<BarcodeDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public DbSet<Barcode> Barcode { get; set; }
        public DbSet<BarcodeHistory> BarcodeHistory { get; set; }

        public DbSet<ModuleDirectory> ModuleDirectory { get; set; }

        public DbSet<ObjectDirectory> ObjectDirectory { get; set; }

        public new async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }

    }
}
