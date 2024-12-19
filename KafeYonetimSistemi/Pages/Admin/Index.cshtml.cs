using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using System.Linq;

namespace KafeYonetimSistemi.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<int> DailyOrders { get; set; } = new List<int>();
        public List<int> WeeklyOrders { get; set; } = new List<int>();
        public List<int> MonthlyOrders { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            // 1 günlük siparişler
            var startOfDay = DateTime.Today;
            DailyOrders = await _context.Order
                .Where(o => o.OrderTime >= startOfDay && o.OrderTime < startOfDay.AddDays(1))
                .GroupBy(o => o.Status)
                .Select(g => g.Count())
                .ToListAsync();

            // 1 haftalık siparişler
            var startOfWeek = DateTime.Today.AddDays(-7);
            WeeklyOrders = await _context.Order
                .Where(o => o.OrderTime >= startOfWeek && o.OrderTime < DateTime.Today.AddDays(1))
                .GroupBy(o => o.Status)
                .Select(g => g.Count())
                .ToListAsync();

            // 1 aylık siparişler
            var startOfMonth = DateTime.Today.AddMonths(-1);
            MonthlyOrders = await _context.Order
                .Where(o => o.OrderTime >= startOfMonth && o.OrderTime < DateTime.Today.AddDays(1))
                .GroupBy(o => o.Status)
                .Select(g => g.Count())
                .ToListAsync();
        }
    }
}
