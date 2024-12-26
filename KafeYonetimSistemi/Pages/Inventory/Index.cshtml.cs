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
            MenuItem = await _context.MenuItem
                                  .Include(m => m.MenuItemTransaction)
                                  .ToListAsync();

        }

    }
}
