using Bluetech.Data;
using Bluetech.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>

{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDataProtection();
builder.Services.AddSingleton<IArithmeticCaptchaService, ArithmeticCaptchaService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;

    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Content-Security-Policy"] = string.Join(' ',
        "default-src 'self';",
        "script-src 'self';",
        "style-src 'self';",
        "img-src 'self' data:;",
        "font-src 'self';",
        "object-src 'none';",
        "base-uri 'self';",
        "frame-ancestors 'none'");

    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var path = ctx.File.PhysicalPath.Replace('\\', '/').ToLowerInvariant();

        if (path.EndsWith(".css", StringComparison.Ordinal) ||
            path.EndsWith(".js", StringComparison.Ordinal) ||
            path.EndsWith(".png", StringComparison.Ordinal) ||
            path.EndsWith(".jpg", StringComparison.Ordinal) ||
            path.EndsWith(".jpeg", StringComparison.Ordinal) ||
            path.EndsWith(".gif", StringComparison.Ordinal) ||
            path.EndsWith(".svg", StringComparison.Ordinal) ||
            path.EndsWith(".webp", StringComparison.Ordinal) ||
            path.EndsWith(".woff", StringComparison.Ordinal) ||
            path.EndsWith(".woff2", StringComparison.Ordinal))
        {
            ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=31536000, immutable";
        }
        else
        {
            ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=600";
        }
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/sitemap.xml", async context =>
{
    context.Response.ContentType = "application/xml; charset=utf-8";
    var now = DateTime.UtcNow.ToString("yyyy-MM-dd");
    var xml = $@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>
< urlset xmlns =\"http://www.sitemaps.org/schemas/sitemap/0.9\">
  < url >< loc > https://bluetech.az/</loc><lastmod>{now}</lastmod><changefreq>weekly</changefreq><priority>1.0</priority></url>
  < url >< loc > https://bluetech.az/Services</loc><lastmod>{now}</lastmod><changefreq>monthly</changefreq><priority>0.8</priority></url>
  < url >< loc > https://bluetech.az/Projects</loc><lastmod>{now}</lastmod><changefreq>weekly</changefreq><priority>0.8</priority></url>
  < url >< loc > https://bluetech.az/Home/About</loc><lastmod>{now}</lastmod><changefreq>yearly</changefreq><priority>0.5</priority></url>
  < url >< loc > https://bluetech.az/Home/Contact</loc><lastmod>{now}</lastmod><changefreq>yearly</changefreq><priority>0.5</priority></url>
</ urlset > ";
    await context.Response.WriteAsync(xml);
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
  
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();