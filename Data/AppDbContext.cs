using Microsoft.EntityFrameworkCore;
using CardPayment.Models;

namespace CardPayment.Data
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Login> Masters { get; set; }
        public DbSet<LCardTempDET> LCardTempDET { get; set; }
        public DbSet<LOYALTYCARD> LOYALTYCARD { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>().ToTable("MASTER", "dbo");
            modelBuilder.Entity<Login>().HasKey(m => m.AID);

            modelBuilder.Entity<LCardTempDET>().ToTable("LCardTempDET", "dbo");
            modelBuilder.Entity<LCardTempDET>().HasKey(m => m.DLCARDID);

            modelBuilder.Entity<LOYALTYCARD>().ToTable("LOYALTYCARD", "dbo");
            modelBuilder.Entity<LOYALTYCARD>().HasKey(m => m.LCARDID);
        }
    }
}
