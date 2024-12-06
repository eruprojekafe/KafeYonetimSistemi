﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;

namespace KafeYonetimSistemi.Pages.QrCode
{
    public class QrCodeModel : PageModel
    {
        private readonly KafeYonetimSistemi.Data.ApplicationDbContext _context;

        public QrCodeModel(KafeYonetimSistemi.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MenuItem> MenuItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            MenuItem = await _context.MenuItem.ToListAsync();
        }
    }
}
