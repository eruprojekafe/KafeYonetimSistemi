using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using Microsoft.EntityFrameworkCore;

namespace KafeYonetimSistemi.Pages.Inventory
{
    public class UpdateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UpdateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MenuItemTransaction MenuItemTransaction { get; set; } = default!;
        public MenuItem? MenuItem { get; set; }

        public IActionResult OnGet(int id)
        {
            MenuItem = _context.MenuItem.FirstOrDefault(m => m.Id == id);

            if (MenuItem == null)
            {
                return NotFound();
            }

            MenuItemTransaction = new MenuItemTransaction
            {
                MenuItemId = id // ID değerini burada ayarlıyoruz
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Önce mevcut stok miktarını al
            var lastTransaction = await _context.MenuItemTransaction
                .Where(m => m.MenuItemId == MenuItemTransaction.MenuItemId)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();

            var currentStock = lastTransaction?.Amount ?? 0; // Mevcut stok miktarı

            // Transaction işlemi
            if (MenuItemTransaction.TransactionType == TransactionType.ADD)
            {
                // Stok artırılır
                MenuItemTransaction.Amount = currentStock + MenuItemTransaction.Amount;
            }
            else if (MenuItemTransaction.TransactionType == TransactionType.REMOVE)
            {
                // Stok azaltılır
                if (currentStock >= MenuItemTransaction.Amount)
                {
                    MenuItemTransaction.Amount = currentStock - MenuItemTransaction.Amount;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Yetersiz stok.");
                    return Page();
                }
            }

            MenuItemTransaction.Timestamp = DateTime.Now;

            // İşlemi veritabanına ekle
            _context.MenuItemTransaction.Add(MenuItemTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
