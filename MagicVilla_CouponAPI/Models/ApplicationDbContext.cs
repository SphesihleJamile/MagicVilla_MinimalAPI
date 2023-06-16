using Microsoft.EntityFrameworkCore;

namespace MagicVilla_CouponAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public ApplicationDbContext()
        {

        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("server=ULINN-DELL\\SQLEXPRESS01;Database=MinimalApi1_MagicVillaVouponProj;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>()
                .HasData(
                    new Coupon()
                    {
                        Id = 1,
                        IsActive = true,
                        Name = "10OFF",
                        Percent = 10
                    },
                    new Coupon()
                    {
                        Id = 2,
                        IsActive = false,
                        Name = "20OFF",
                        Percent = 20
                    }
                );
        }
    }
}
