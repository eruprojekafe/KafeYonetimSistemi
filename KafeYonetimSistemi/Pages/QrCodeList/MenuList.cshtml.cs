using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using Microsoft.AspNetCore.Mvc;

namespace KafeYonetimSistemi.Pages.MenuList
{
    public class MenuListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MenuListModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int TableNumber { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public async Task<IActionResult> OnGetAsync(int categoryId, int tableNumber)
        {
            TableNumber = tableNumber;  // tableNumber'ı modelin içine alıyoruz

            // Kategori adına erişim
            var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                // Eğer kategori bulunamazsa, kullanıcıyı ana sayfaya yönlendir
                return RedirectToPage("/QrCodeList");
            }

            CategoryName = category.Name;

            // Seçilen kategoriye ait menü öğelerini getir
            MenuItems = await _context.MenuItem
                .Where(m => m.CategoryId == categoryId)
                .ToListAsync();

            return Page();
        }
    }
}
