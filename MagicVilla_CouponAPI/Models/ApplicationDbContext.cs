using Microsoft.EntityFrameworkCore;

namespace MagicVilla_CouponAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }

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
