using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PetHouse.Data;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using PetHouse.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddNotyf(configure =>
{
    configure.DurationInSeconds = 5;
    configure.IsDismissable = true;
    configure.Position = NotyfPosition.BottomRight;
});

builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.AddDbContext<PetHouseDbContext>(option => option.UseSqlServer(
builder.Configuration.GetConnectionString("PetHouseDb")
    ));
builder.Services.AddNotyf(config =>
{
	config.DurationInSeconds = 3;
	config.IsDismissable = true;
	config.Position = NotyfPosition.TopRight;
});
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<PetHouseDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "/Admin/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");
//AppDbInitializer.SeedUsersAndRoleAsync(app).Wait();
app.Run();
