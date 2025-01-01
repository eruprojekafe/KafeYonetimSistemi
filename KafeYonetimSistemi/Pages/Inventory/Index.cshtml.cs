using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

namespace KafeYonetimSistemi.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public IndexModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MenuItem> MenuItem { get; set; }
        public IList<MenuItemTransaction> MenuItemTransaction { get; set; }

        public async Task OnGetAsync()
        {
            // Menü öğelerini ve ilişkili işlemleri yükle
            MenuItem = await _context.MenuItem
                .Include(m => m.MenuItemTransaction)
                .ToListAsync();
        }

        public decimal CurrentAmount { get; set; }
        public int GetCurrentAmount(int id)
        {
            // İlgili işlemlerden stok miktarını hesapla
            return (int)_context.MenuItemTransaction
                .Where(t => t.MenuItemId == id)
                .Sum(t => t.TransactionType == TransactionType.ADD ? t.Amount : -t.Amount);
        }
    }
}
