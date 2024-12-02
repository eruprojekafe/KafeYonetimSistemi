using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Models;

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
    }
}
