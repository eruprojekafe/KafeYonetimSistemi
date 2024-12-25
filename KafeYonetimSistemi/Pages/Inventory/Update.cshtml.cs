using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

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
        public MenuItem MenuItem { get; set; } = default!;



        public IActionResult OnGet(int id)
        {
            // MenuItem nesnesini id ile alıyoruz.
            MenuItem = _context.MenuItem.FirstOrDefault(m => m.Id == id);

            if (MenuItem == null)
            {
                return NotFound(); // Eğer ürün bulunamazsa hata döndür
            }

            // MenuItemTransaction için varsayılan yapı oluştur
            MenuItemTransaction = new MenuItemTransaction
            {
                MenuItemId = id, // Alınan MenuItem'ın Id'si ile eşleştir
                Amount = 0, // Varsayılan stok miktarı
                TransactionType = TransactionType.ADD // Varsayılan işlem tür
            };

            return Page();
        }



        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // İşlem tarihini güncelle
            MenuItemTransaction.Timestamp = DateTime.Now;

            // İşlem türüne göre stok miktarını güncelle
            var menuItem = await _context.MenuItem.FindAsync(MenuItemTransaction.MenuItemId);
            if (menuItem == null)
            {
                return NotFound();
            }

            if (MenuItemTransaction.TransactionType == TransactionType.ADD)
            {
                menuItem.Price += MenuItemTransaction.Amount; // Stok miktarını artır
            }
            else if (MenuItemTransaction.TransactionType == TransactionType.REMOVE)
            {
                menuItem.Price -= MenuItemTransaction.Amount; // Stok miktarını azalt
                if (menuItem.Price < 0) // Negatif stok miktarı kontrolü
                {
                    menuItem.Price = 0;
                }
            }

            // Stok değişikliğini kaydet
            _context.MenuItem.Update(menuItem);


            // İşlemi veritabanına ekle
            _context.MenuItemTransaction.Add(MenuItemTransaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ürün başarıyla güncellendi!";

            return RedirectToPage("./Index");
        }
    }
}
