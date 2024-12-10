using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KafeYonetimSistemi.Middleware
{
    public class AdminAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // İstek URL'si "/Admin" ile başlıyorsa
            if (context.Request.Path.StartsWithSegments("/Admin"))
            {
                // 404 Hata döndür
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("404 - Sayfa Bulunamadı");
                return;
            }

            // Diğer middleware'lere geç
            await _next(context);
        }
    }
}

