using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KafeYonetimSistemi.Pages.Admin.MenuItems
{
    public class CreateModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public CreateModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        // Seçilebilir Kategoriler
        public List<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();

        // MenuItem ve Category ilişkisi
        [BindProperty]
        public MenuItem MenuItem { get; set; } = default!;

        public IActionResult OnGet()
        {
            // Kategorileri listele
            AvailableCategories = _context.Category
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name // Kategori adı
                }).ToList();

            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        // Menü öğesi oluşturma işlemi
        public async Task<IActionResult> OnPostAsync()
        {
            var category = _context.Category.FirstOrDefault(c => c.Id == MenuItem.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("MenuItem.Category", "Geçersiz kategori seçimi.");
                return OnGet();
            }
            MenuItem.Category = category;

            ModelState.Clear();
            TryValidateModel(MenuItem);

            if (!ModelState.IsValid)
            {
                return OnGet();
            }

            // Menü öğesini veritabanına ekle
            _context.MenuItem.Add(MenuItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
