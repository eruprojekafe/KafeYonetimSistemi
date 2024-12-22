using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

namespace KafeYonetimSistemi.Pages.MenuList
{
    public class MenuListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MenuListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CategoryName { get; set; } = string.Empty;
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public async Task OnGetAsync(int categoryId)
        {
            // Kategori adına erişim
            var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category != null)
            {
                CategoryName = category.Name;
            }

            // Seçilen kategoriye ait menü öğelerini getir
            MenuItems = await _context.MenuItem
                .Where(m => m.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}