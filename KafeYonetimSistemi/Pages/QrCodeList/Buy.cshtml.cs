using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Models;
using KafeYonetimSistemi.Pages.QrCodeList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KafeYonetimSistemi.Pages.QrCodeList
{
    public class BuyModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BuyModel(ApplicationDbContext context)
        {
            _context = context;
            PaymentInfo = new PaymentData(); // Initialize PaymentInfo to avoid null reference issues
        }
        public decimal TotalAmount { get; set; }
        [BindProperty]
        public PaymentData PaymentInfo { get; set; }

        public string Message { get; set; } = string.Empty; // Initialize Message to avoid null reference

        [BindProperty(SupportsGet = true)]
        public int TableNumber { get; set; }

        [BindProperty]
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>(); // Ensure CartItems is always initialized

        public IActionResult OnGet()
        {

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Ödeme bilgileri geçerli deðil.";
                return Page();
            }

            if (SimulatePayment(PaymentInfo))
            {
                try
                {
                    SaveOrderToDatabase(CartItems);
                    Message = "Ödeme baþarýlý. Sipariþiniz oluþturuldu!";
                }
                catch (Exception ex)
                {
                    Message = $"Sipariþ kaydedilirken hata oluþtu: {ex.Message}";
                }
            }
            else
            {
                Message = "Ödeme baþarýsýz. Lütfen kart bilgilerinizi kontrol edin.";
            }

            return Page();
        }

        private bool SimulatePayment(PaymentData paymentData)
        {
            return paymentData.CardNumber == "1234 5678 9012 3456";
        }

        private void SaveOrderToDatabase(List<CartItemDto> cartItems)
        {
            var table = _context.Table.FirstOrDefault(t => t.TableNumber == TableNumber);

            if (table == null)
            {
                throw new Exception($"Tablo numarasý {TableNumber} bulunamadý!");
            }

            var order = new Order
            {
                OrderTime = DateTime.Now,
                Status = OrderStatus.Created,
                Table = table,
                MenuItems = cartItems.Select(cartItem => new MenuItem
                {
                    Id = cartItem.MenuItemId,
                    Name = cartItem.Name ?? "Bilinmeyen",
                    Price = cartItem.Price
                }).ToList()
            };

            _context.Order.Add(order);
            _context.SaveChanges();
        }
        public class PaymentData
        {
            public string? CardNumber { get; set; }
            public string? CardName { get; set; }
            public string? Expiration { get; set; }
            public string? CVV { get; set; }
        }
    }
}