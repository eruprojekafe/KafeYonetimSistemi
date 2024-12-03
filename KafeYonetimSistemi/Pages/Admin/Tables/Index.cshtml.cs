using KafeYonetimSistemi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KafeYonetimSistemi.Pages.Admin.Tables
{
    public class IndexModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public IndexModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Table> Table { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Table = await _context.Table.ToListAsync();
        }
    }
}
