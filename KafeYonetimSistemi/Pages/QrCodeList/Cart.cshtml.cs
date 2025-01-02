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

        // Kullan�c�ya g�nderilecek �r�nlerin listesi
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

        // Sepet sayfas�n� y�klendi�inde �a�r�lacak
        public IActionResult OnGetAsync(int tableNumber)
        {
            TableNumber = tableNumber;
            return Page();
        }

        // Sepet verilerini sunucuya POST i�lemiyle al ve i�leme koy
        public IActionResult OnPost([FromBody] List<CartItemDto> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest(new { message = "Sepet bo�!" });
            }

            // G�nderilen MenuItemId'lere g�re veritaban�ndan �r�n bilgilerini al
            var items = _context.MenuItem
                                .Where(item => cartItems.Select(ci => ci.MenuItemId).Contains(item.Id))
                                .ToList();

            if (items.Count == 0)
            {
                return BadRequest(new { message = "�r�nler bulunamad�!" });
            }

            // Sepetteki �r�nlerin detaylar�n� g�ncelle
            CartItems = cartItems.Select(cartItem =>
            {
                var dbItem = items.FirstOrDefault(i => i.Id == cartItem.MenuItemId);
                return new CartItemDto
                {
                    MenuItemId = cartItem.MenuItemId,
                    Name = dbItem?.Name ?? "Bilinmeyen �r�n",
                    Price = dbItem?.Price ?? 0,
                    Quantity = cartItem.Quantity
                };
            }).ToList();

            // ��lem ba�ar�l�
            return new JsonResult(new { message = "Sepet ba�ar�yla i�lendi.", CartItems });
        }

    }

    // Sepet ��esi i�in DTO s�n�f�
    public class CartItemDto
    {
        public int MenuItemId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}