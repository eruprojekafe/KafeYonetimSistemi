using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Newtonsoft.Json;

namespace KafeYonetimSistemi.Pages.QrCodeList
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CartModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int TableNumber { get; set; }
        public decimal TotalAmount { get; set; }

        // Kullanýcýya gönderilecek ürünlerin listesi
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

        // Sepet sayfasýný yüklendiðinde çaðrýlacak
        public IActionResult OnGetAsync(int tableNumber)
        {
            TableNumber = tableNumber;
            return Page();
        }

        // Sepet verilerini sunucuya POST iþlemiyle al ve iþleme koy
        public IActionResult OnPost([FromBody] List<CartItemDto> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest(new { message = "Sepet boþ!" });
            }

            // Gönderilen MenuItemId'lere göre veritabanýndan ürün bilgilerini al
            var items = _context.MenuItem
                                .Where(item => cartItems.Select(ci => ci.MenuItemId).Contains(item.Id))
                                .ToList();

            if (items.Count == 0)
            {
                return BadRequest(new { message = "Ürünler bulunamadý!" });
            }

            // Sepetteki ürünlerin detaylarýný güncelle
            CartItems = cartItems.Select(cartItem =>
            {
                var dbItem = items.FirstOrDefault(i => i.Id == cartItem.MenuItemId);
                return new CartItemDto
                {
                    MenuItemId = cartItem.MenuItemId,
                    Name = dbItem?.Name ?? "Bilinmeyen Ürün",
                    Price = dbItem?.Price ?? 0,
                    Quantity = cartItem.Quantity
                };
            }).ToList();

            // Ýþlem baþarýlý
            return new JsonResult(new { message = "Sepet baþarýyla iþlendi.", CartItems });
        }

    }

    // Sepet öðesi için DTO sýnýfý
    public class CartItemDto
    {
        public int MenuItemId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}