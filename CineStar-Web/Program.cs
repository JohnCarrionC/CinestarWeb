using CineStar.Data;

var builder = WebApplication.CreateBuilder(args);

// Archivo local (NO se sube a git) con la cadena de conexión para desarrollo.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<CineStarRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
