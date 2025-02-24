using ERPIntegration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services to the container.
builder.Services.AddControllersWithViews();

// Register HttpClient and our ERP integration service.
builder.Services.AddHttpClient<IErpIntegrationService, ErpIntegrationService>();
builder.Services.AddScoped<IErpIntegrationService, ErpIntegrationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Set the default route to HomeController's Index action.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
