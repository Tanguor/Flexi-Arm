using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Serilog;
using Microsoft.AspNetCore.SignalR.Client;
using Flexi_Arm.Models;
using Flexi_Arm.Controllers;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add authorization policies
AddAuthorizationPolicies(builder.Services);

// Add SignalR services
builder.Services.AddSignalR();

// Configure the default recipe Id
var defaultRecetteId = Configuration.GetValue<int>("DefaultRecetteId");
builder.Services.Configure<DefaultRecetteOptions>(options => options.Id = defaultRecetteId);
Console.WriteLine($"DefaultRecetteId: {defaultRecetteId}");

// Add DbContext
var connectionString = Configuration.GetConnectionString("ApplicationDbContextConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add Serilog
var _logger = new LoggerConfiguration()
    .WriteTo.File("Logs\\FlexiArmLog-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog(_logger);


// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); //J'ai retiré ça car cela casse mon app quand je la publie !
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map SignalR hub
app.MapHub<CameraHub>("/cameraHub");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Add authorization policies
void AddAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
        options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Administrateur"));
        options.AddPolicy("RequireMaintenance", policy => policy.RequireRole("Maintenance"));
    });
}