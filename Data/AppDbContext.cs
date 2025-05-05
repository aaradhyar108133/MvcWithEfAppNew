using Microsoft.EntityFrameworkCore;
using MvcWithEfApp.Models;

namespace MvcWithEfApp.Data
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Master> Masters { get; set; }
        public DbSet<LCardTempDET> LCardTempDET { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Master>().ToTable("MASTER", "dbo");
            modelBuilder.Entity<Master>().HasKey(m => m.AID);

            modelBuilder.Entity<LCardTempDET>().ToTable("LCardTempDET", "dbo");
            modelBuilder.Entity<LCardTempDET>().HasKey(m => m.DLCARDID);
        }
    }
}
