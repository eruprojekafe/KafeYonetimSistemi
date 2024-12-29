using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabani baglanti dizesini al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ApplicationDbContext'i yapilandir
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Gelistirici hatalarini gostermek icin middleware ekleme
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Kimlik dogrulama ve kullanici yonetimi icin Identity yapilandirmasi
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages destegi ekleme ve yetkilendirme ayarlari
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Waiters");  // Garson klasorunu yetkilendir
});

var app = builder.Build();

// Admin erisim middleware'ini kullan
app.UseMiddleware<AdminAccessMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Gelistirme ortaminda migration endpoint'i etkinlestir
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    // Uretim ortaminda hata isleme
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// HTTPS yonlendirmeleri ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Waiters klasorune yetkisiz erisimi engelleyen middleware
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/Waiters") && !context.User.Identity.IsAuthenticated)
    {
        // Yetkisiz kullaniciya 404 hatasý dondur
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("404 - Page Not Found");
        return;
    }
    else
    {
        await next();
    }
});

// Yetkilendirme
app.UseAuthorization();

// Razor Pages icin rota ekleme
app.MapRazorPages();

app.Run();
