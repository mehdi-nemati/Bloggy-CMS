using Microsoft.AspNetCore.Identity;
using Bloggy.Data;
using Bloggy.Entities;
using Autofac;
using Bloggy.Web.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Bloggy.Shared;
using Microsoft.Extensions.Localization;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.AddServices());

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                   .AddDefaultTokenProviders()
                   .AddEntityFrameworkStores<BloggyDbContext>();

builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization(options =>
    options.DataAnnotationLocalizerProvider = (type, factory) => new StringLocalizer<SharedResources>(factory));

builder.Services.AddRazorPages();

builder.Services.InitializeAutoMapper();
builder.Services.AddScoped<IStringLocalizer, StringLocalizer<SharedResources>>();

builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("fa-IR"),
                            new CultureInfo("es-ES"),
                        };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});


var app = builder.Build();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseHttpsRedirection();

// Use static files
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 30 days
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=2592000");
        ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(30).ToString("R", CultureInfo.InvariantCulture));
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();