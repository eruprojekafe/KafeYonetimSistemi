using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KafeYonetimSistemi.Middleware
{
    public class WaitersAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public WaitersAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (context.Request.Path.StartsWithSegments("/Waiters"))
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
