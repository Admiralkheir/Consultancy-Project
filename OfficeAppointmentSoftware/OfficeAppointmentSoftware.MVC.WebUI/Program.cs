using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeAppointmentSoftware.BLL.Abstracts;
using OfficeAppointmentSoftware.BLL.Abstracts.AdminServices;
using OfficeAppointmentSoftware.BLL.Concrete;
using OfficeAppointmentSoftware.BLL.Concrete.AdminServices;
using OfficeAppointmentSoftware.DAL.Contexts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts;
using OfficeAppointmentSoftware.DAL.Repositories.Abstracts.AdminRepositories;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes;
using OfficeAppointmentSoftware.DAL.Repositories.Concretes.AdminRepositories;
using OfficeAppointmentSoftware.MVC.WebUI.Infrastructure;
using OfficeAppointmentSoftware.MVC.WebUI.Models.Configurations;
using OfficeAppointmentSoftware.MVC.WebUI.Models.IdentityModels;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AppointmentDBContext")
    ?? throw new InvalidOperationException("Connection string 'AppointmentDBContext' is not configured.");

builder.Services.AddDbContext<AppointmentDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        // ASP.NET Identity 2 had no password rules at all; keep a sane minimum without the Core defaults' strictness.
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IFeaturesRepository, FeaturesRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IWorksRepository, WorksRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IExpertRepository, ExpertRepository>();

builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IFeaturesService, FeaturesService>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<IWorksService, WorksService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IExpertService, ExpertService>();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

// Razor would otherwise emit Turkish letters as entities (ş -> &#x15F;), which shows up raw inside scripts such as confirm("@ViewBag.Mesaj").
builder.Services.AddWebEncoders(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

builder.Services
    .AddControllersWithViews(options =>
    {
        // Every POST must carry an antiforgery token (forms get it from the form tag helper, the calendar sends it as a header).
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        TurkishValidationMetadataProvider.Configure(options);
    })
    // The calendar script reads PascalCase property names, as ASP.NET MVC 5 produced them.
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        // Turkish letters as-is instead of ş escapes (HTML-sensitive characters are still escaped).
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services, app.Configuration, app.Logger);

// Same as <globalization culture="en-GB"> in the old Web.config: dates are posted as dd/MM/yyyy.
var culture = new CultureInfo("en-GB");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = [culture],
    SupportedUICultures = [culture]
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
