using KafeYonetimSistemi.Data;
using KafeYonetimSistemi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant� dizesini al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ApplicationDbContext'i yap�land�r
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Geli�tirici hatalar�n� g�stermek i�in middleware ekleme
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Kimlik do�rulama ve kullan�c� y�netimi i�in Identity yap�land�rmas�
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages deste�i ekleme
builder.Services.AddRazorPages();

var app = builder.Build();

// Admin eri�im middleware'ini kullan
app.UseMiddleware<AdminAccessMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Geli�tirme ortam�nda migration endpoint'i etkinle�tir
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    // �retim ortam�nda hata i�leme
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// HTTPS y�nlendirmeleri ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Yetkilendirme
app.UseAuthorization();

// Razor Pages i�in rota ekleme
app.MapRazorPages();

app.Run();

