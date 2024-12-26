using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

namespace KafeYonetimSistemi.Pages.Admin.MenuItems
{
    public class UpdateModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public UpdateModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MenuItemTransaction MenuItemTransaction { get; set; } = default!;
        public MenuItem MenuItem { get; set; } = default!;
        public decimal CurrentAmount { get; set; }
        public IActionResult OnGet(int id)
        {
            // İlgili MenuItem ve mevcut stok durumunu al
            MenuItem = _context.MenuItem.FirstOrDefault(m => m.Id == id);

            if (MenuItem == null)
            {
                return NotFound();
            }

            CurrentAmount = _context.MenuItemTransaction
                .Where(t => t.MenuItemId == id)
                .Sum(t => t.TransactionType == TransactionType.ADD ? t.Amount : -t.Amount);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // İşlemi kaydet
            MenuItemTransaction.Timestamp = DateTime.Now;
            _context.MenuItemTransaction.Add(MenuItemTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
