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

        public IActionResult OnGetAsync(int tableNumber, string cartItems)
        {
            // Deserialize the cartItems JSON to a list of CartItemDto objects
            if (!string.IsNullOrEmpty(cartItems))
            {
                CartItems = JsonConvert.DeserializeObject<List<CartItemDto>>(cartItems);
            }

            // Set the table number received from the URL
            TableNumber = tableNumber;

            // Calculate the total amount
            TotalAmount = CartItems.Sum(c => c.Quantity * c.Price);

            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine($"TableNumber: {TableNumber}");
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
            // Kart numarasý Luhn algoritmasýna göre kontrol edilir.
            if (!IsValidCardNumber(paymentData.CardNumber))
            {
                return false;
            }
            var cleanedExpiration = paymentData.Expiration.Replace(" ", ""); // Boþluklarý kaldýr
            if (!DateTime.TryParseExact(cleanedExpiration, "MM/yy", null, System.Globalization.DateTimeStyles.None, out var expirationDate))
            {
                return false;
            }

            if (expirationDate < DateTime.Now.AddMonths(-1))
            {
                return false;
            }

            // CVV kontrolü
            if (string.IsNullOrWhiteSpace(paymentData.CVV) || paymentData.CVV.Length < 3 || paymentData.CVV.Length > 4)
            {
                return false;
            }

            return true; // Simüle edilmiþ baþarý
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", ""); // Boþluklarý kaldýr
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return false;
            }

            if (!cardNumber.All(char.IsDigit)) // Sadece rakamlarý kontrol et
            {
                return false;
            }
            if (cardNumber.Length != 16) // Sadece 16 haneli kartlar kabul edilir
            {
                return false;
            }
            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                if (!int.TryParse(cardNumber[i].ToString(), out int n))
                {
                    return false;
                }

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        private void SaveOrderToDatabase(List<CartItemDto> cartItems)
        {
            var table = _context.Table.FirstOrDefault(t => t.TableNumber == TableNumber);
            if (table == null)
            {
                throw new Exception($"Tablo numarasý {TableNumber} bulunamadý!");
            }

            // Gerekli MenuItem'leri tek seferde veritabanýndan çek
            var menuItemIds = cartItems.Select(c => c.MenuItemId).ToList();
            var menuItems = _context.MenuItem.Where(m => menuItemIds.Contains(m.Id)).ToDictionary(m => m.Id);

            // Yeni sipariþ oluþtur
            var order = new Order
            {
                OrderTime = DateTime.Now,
                Status = OrderStatus.Created,
                Table = table, // Tablo numarasýný sipariþle iliþkilendiriyoruz
                MenuItems = new List<MenuItem>()
            };

            foreach (var cartItem in cartItems)
            {
                if (!menuItems.TryGetValue(cartItem.MenuItemId, out var menuItem))
                {
                    throw new Exception($"MenuItem ID {cartItem.MenuItemId} bulunamadý!");
                }

                order.MenuItems.Add(menuItem);
            }

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