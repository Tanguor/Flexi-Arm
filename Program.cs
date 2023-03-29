using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Flexi_Arm.Areas.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

var builder = WebApplication.CreateBuilder(args);
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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=LogPage}/{id?}");

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
    IWebHostEnvironment HostingEnv;
    public Startup(IWebHostEnvironment env)
    {
        HostingEnv = env;
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        builder.AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => {
            var loggingSection = Configuration.GetSection("Logging");
            loggingBuilder.AddConfiguration(loggingSection);
            loggingBuilder.AddConsole();

            Action<FileLoggerOptions> resolveRelativeLoggingFilePath = (fileOpts) => {
                fileOpts.FormatLogFileName = fName => {
                    return Path.IsPathRooted(fName) ? fName : Path.Combine(HostingEnv.ContentRootPath, fName);
                };
            };

            loggingBuilder.AddFile(loggingSection.GetSection("FileOne"), resolveRelativeLoggingFilePath);
            loggingBuilder.AddFile(loggingSection.GetSection("FileTwo"), resolveRelativeLoggingFilePath);

            // alternatively, you can configure 2nd file logger (or both) in the code:
            /*loggingBuilder.AddFile("logs/app_debug.log", (fileOpts) => {
                fileOpts.MinLevel = LogLevel.Debug;
                resolveRelativeLoggingFilePath(fileOpts);
            });*/

        });

        services.AddMvc(options => {
            options.EnableEndpointRouting = false;
        });

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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
