﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public MenuItem? MenuItem { get; set; }

        private bool Initialize(int id)
        {
            MenuItem = _context.MenuItem.FirstOrDefault(m => m.Id == id);

            if (MenuItem == null)
            {
                return false;
            }

            MenuItemTransaction = new MenuItemTransaction()
            {
                MenuItemId = id,
            };

            return true;
        }

        public IActionResult OnGet(int id)
        {
            if (!Initialize(id))
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (MenuItemTransaction.Amount <= 0)
            {
                ModelState.AddModelError("MenuItemTransaction.Amount", "Miktar değeri negatif veya sıfır olamaz.");
                return OnGet(MenuItemTransaction.MenuItemId);
            }

            if (!TryValidateModel(MenuItemTransaction))
            {
                return OnGet(MenuItemTransaction.MenuItemId);
            }

            MenuItemTransaction.Timestamp = DateTime.Now;

            // İşlemi veritabanına ekle
            _context.MenuItemTransaction.Add(MenuItemTransaction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}