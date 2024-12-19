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
        public IList<Category> Categories { get; set; } = new List<Category>();

        public async Task OnGetAsync()
        {
            // Tüm kategorileri getir
            Categories = await _context.Category
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl // Resim ve isim bilgisi
                })
                .ToListAsync();
        }
    }
}
