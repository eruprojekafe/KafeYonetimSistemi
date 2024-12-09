using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

namespace KafeYonetimSistemi.Pages.QrCodeList
{
    public class QrCodeListModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public QrCodeListModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public int TableNumber { get; set; }
        public int? SelectedCategoryId { get; set; }

        public IList<MenuItem> MenuItem { get; set; } = new List<MenuItem>();
        public IList<Category> Categories { get; set; } = new List<Category>();

        public async Task OnGetAsync(int tableNumber, int? categoryId)
        {
            TableNumber = tableNumber;
            SelectedCategoryId = categoryId;

            // Kategorileri çekecek 
            Categories = await _context.Category.ToListAsync();

            // Menü öğelerini kategoriye göre filtreleyecek 
            if (categoryId.HasValue)
            {
                MenuItem = await _context.MenuItem
                    .Where(m => m.Id == categoryId.Value)
                    .ToListAsync();
            }
            else
            {
                MenuItem = await _context.MenuItem.ToListAsync();
            }
        }
    }
}
