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

            if (context.Request.Path.StartsWithSegments("/Admin"))
            {

                if (!context.User.Identity.IsAuthenticated)
                {

                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("404 - Page not Found");
                    return;
                }
            }


            await _next(context);
        }
    }
}


