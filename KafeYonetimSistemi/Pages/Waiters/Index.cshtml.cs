using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Models;
using KafeYonetimSistemi.Data;


namespace KafeYonetimSistemi.Pages.Waiters
{

    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task OnGetAsync()
        {
            // Teslim edilmemis siparisleri getir
            Orders = await _context.Order
                .Where(o => o.Status != OrderStatus.Delivered) // Enum ile filtreleme
                .Include(o => o.MenuItems) // Menu ogelerini de dahil et
                .Include(o => o.Table)     // Masayi da dahil et
                .ToListAsync();
        }
    }
}