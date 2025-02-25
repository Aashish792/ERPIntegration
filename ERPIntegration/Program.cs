using ERPIntegration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load configuration from appsettings.json
var configuration = builder.Configuration;

// ✅ Add logging to help debug session issues
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ✅ Register services
builder.Services.AddControllersWithViews();

// ✅ Register session services
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Security best practice
    options.Cookie.IsEssential = true; // Ensures session works without user consent
});

// ✅ Register the ERP service for dependency injection with configuration support
builder.Services.AddHttpClient<IErpIntegrationService, ErpIntegrationService>();
builder.Services.AddSingleton<IConfiguration>(configuration);

var app = builder.Build();

// ✅ Enable session BEFORE authorization
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ✅ Set up the default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
