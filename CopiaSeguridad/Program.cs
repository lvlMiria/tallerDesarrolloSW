using Microsoft.EntityFrameworkCore;
using Presentacion.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SistPresupuestosContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion"))
        .EnableSensitiveDataLogging()); //Quitar luego

//var infoCultura = new CultureInfo("es-ES");
//infoCultura.NumberFormat.NumberDecimalSeparator = ",";
//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(infoCultura);
//    options.SupportedCultures = new List<CultureInfo> { infoCultura };
//    options.SupportedUICultures = new List<CultureInfo> { infoCultura };
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
