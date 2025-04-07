using Booking_Halls.Application;
using Booking_Halls.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Hangfire;
using Hangfire.PostgreSql;
using Booking_Halls.Application.Interfaces;
using Booking_Halls.Endpoints;

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


// 🔸 Добавляем Hangfire + PostgreSQL storage
builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();


var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 🔸 Панель мониторинга Hangfire по адресу /hangfire
app.UseHangfireDashboard("/hangfire");

// 🔹 Повторяющаяся задача: удаление просроченных бронирований каждые 5 минут
RecurringJob.AddOrUpdate<IBookingService>(
    "remove-expired-bookings",
    x => x.RemoveExpiredBookingsAsync(),
    Cron.MinuteInterval(5));

app.UseEndpoints(EndpointConfig.RegisterRoutes);


app.Run();
