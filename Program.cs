using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    //ajout du des roles à notre identité
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

#region 
//ajouté par tanguy

AddAuthorizationPolicies(builder.Services);

#endregion

//utilisation  de serilog pour récupérer un fichier de log.
Log.Logger = new LoggerConfiguration() //version ciblé
    .MinimumLevel.Warning()
//var _logger = new LoggerConfiguration() ( version générale logging général )
.WriteTo.File("Logs\\FlexiArmLog-.log", rollingInterval: RollingInterval.Day)
.CreateLogger();
//builder.Logging.AddSerilog(_logger) (version générale du logging)

builder.Services.AddRazorPages();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

//migration, Commande = Add-Migration InitApplicationUser (ouvrir Package Manager Console)

//Ajout�e par tanguy, authorisation.
void AddAuthorizationPolicies(IServiceCollection services)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Administrateur"));
        options.AddPolicy("RequireMaintenance", policy => policy.RequireRole("Maintenance"));
    });
}


public class Startup
{
    readonly IWebHostEnvironment HostingEnv;
    public Startup(IWebHostEnvironment env)
    {
        HostingEnv = env;
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false);

        builder.AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(options => {
            options.EnableEndpointRouting = false;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configuration de l'environnement d'exécution
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        // Configuration du routage
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=LoggingDemo}/{action=DemoPage}");
        });
    }
}
