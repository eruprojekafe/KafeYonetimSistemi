using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

namespace KafeYonetimSistemi.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public EditModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public List<SelectListItem> OrderStatusList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableTables { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public Order Order { get; set; } = default!;
        private void PopulateFormData()
        {
            // Müsait masaları listeleme
            AvailableTables = _context.Table
                .Where(t => t.IsAvailable)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.ToString()
                }).ToList();

            // Sipariş durumlarını listeleme
            OrderStatusList = Enum.GetValues(typeof(OrderStatus))
               .Cast<OrderStatus>()
               .Select(s => new SelectListItem
               {
                   Value = ((int)s).ToString(),
                   Text = $"{s.GetDescription()}"
               }).ToList();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            // Form verilerini doldur
            PopulateFormData();
            Order = order;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Form verilerini tekrar doldur
                PopulateFormData();
                return Page();
            }
            // Tabloyu direkt atamaktansa Id üzerinden ilişkilendirin
            var table = _context.Table.FirstOrDefault(t => t.Id == Order.Table.Id);
            if (table == null)
            {
                ModelState.AddModelError("Order.Table.Id", "Geçersiz masa seçimi.");
                PopulateFormData();
                return Page();
            }

            Order.Table = table;

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
