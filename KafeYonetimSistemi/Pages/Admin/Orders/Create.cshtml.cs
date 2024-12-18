using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;


namespace KafeYonetimSistemi.Pages.Admin.Orders
{
    public static class OrderStatusExtensions
    {
        public static string GetDescription(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Created => "Oluşturuldu",
                OrderStatus.Preparing => "Hazırlanıyor",
                OrderStatus.Ready => "Hazır",
                OrderStatus.Delivered => "Teslim Edildi",
                OrderStatus.Cancelled => "İptal Edildi",
                _ => "Bilinmiyor"
            };
        }
    }

    public class CreateModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public CreateModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public List<SelectListItem> AvailableTables { get; set; } = new List<SelectListItem>();


        public List<SelectListItem> OrderStatusList { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public Order Order { get; set; } = default!;

        private void GetFormData()
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

        public IActionResult OnGet()
        {
            // Form verilerini doldur
            GetFormData();

            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Geçersiz model durumunda form verilerini tekrar doldur
                GetFormData();

                return Page();
            }

            // Tabloyu direkt atamaktansa Id üzerinden ilişkilendirin
            var table = _context.Table.FirstOrDefault(t => t.Id == Order.Table.Id);
            if (table == null)
            {
                ModelState.AddModelError("Order.Table.Id", "Geçersiz masa seçimi.");
                GetFormData();
                return Page();
            }

            Order.Table = table;

            _context.Order.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
