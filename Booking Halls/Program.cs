using Booking_Halls.Application;
using Booking_Halls.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; 
        options.AccessDeniedPath = "/access-denied"; 
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "login",
        pattern: "{controller=Login}/{action=Login}/{id?}");

    endpoints.MapControllerRoute(
        name: "register",
        pattern: "{controller=Register}/{action=Register}/{id?}");
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{controller=Admin}/{action=Index}/{id?}");
});

app.Run();
