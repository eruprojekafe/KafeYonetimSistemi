using KafeYonetimSistemi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KafeYonetimSistemi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enum'ı integer olarak sakla
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<int>();
        }
        public DbSet<KafeYonetimSistemi.Models.Order> Order { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.Table> Table { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.MenuItem> MenuItem { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.Category> Category { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.MenuItemTransaction> MenuItemTransaction { get; set; } = default!;
    }

}
