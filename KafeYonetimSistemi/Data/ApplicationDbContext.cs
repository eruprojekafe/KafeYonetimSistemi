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
        public DbSet<KafeYonetimSistemi.Models.Order> Order { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.Table> Table { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.MenuItem> MenuItem { get; set; } = default!;
        public DbSet<KafeYonetimSistemi.Models.Category> Category { get; set; } = default!;
    }
}
