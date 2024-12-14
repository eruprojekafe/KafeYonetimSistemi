using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantý dizesini al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ApplicationDbContext'i yapýlandýr
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Geliþtirici hatalarýný göstermek için middleware ekleme
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Kimlik doðrulama ve kullanýcý yönetimi için Identity yapýlandýrmasý
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages desteði ekleme
builder.Services.AddRazorPages();

var app = builder.Build();

// Admin eriþim middleware'ini kullan
app.UseMiddleware<AdminAccessMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Geliþtirme ortamýnda migration endpoint'i etkinleþtir
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    // Üretim ortamýnda hata iþleme
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// HTTPS yönlendirmeleri ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Yetkilendirme
app.UseAuthorization();

// Razor Pages için rota ekleme
app.MapRazorPages();

app.Run();

