/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KafeYonetimSistemi.Pages.Waiters
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
*/
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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
            // Teslim edilmemiþ sipariþleri getir
            Orders = await _context.Order
                .Where(o => o.Status != OrderStatus.Delivered) // Enum ile filtreleme
                .Include(o => o.MenuItems) // Menü öðelerini de dahil et
                .Include(o => o.Table)     // Masayý da dahil et
                .ToListAsync();
        }
    }
}