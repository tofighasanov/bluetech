using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;      // для WriteAsync и Headers
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bluetech
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 1) Ошибки и HSTS
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // 2) HTTPS редирект
            app.UseHttpsRedirection();

            // 3) Статические файлы + базовый кэш
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    var path = ctx.File.PhysicalPath.Replace('\\', '/').ToLowerInvariant();
                    // Долгий кэш для версионируемой статики
                    if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png") ||
                        path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".gif") ||
                        path.EndsWith(".svg") || path.EndsWith(".webp") || path.EndsWith(".woff") ||
                        path.EndsWith(".woff2"))
                    {
                        ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=31536000, immutable";
                    }
                    else
                    {
                        // Для sitemap/robots — короткий кэш
                        ctx.Context.Response.Headers["Cache-Control"] = "public, max-age=600";
                    }
                }
            });

            // 4) Security headers + CSP
            app.Use(async (context, next) =>
            {
                var headers = context.Response.Headers;

                headers["X-Content-Type-Options"] = "nosniff";
                headers["X-Frame-Options"] = "DENY";
                headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

                // Разрешаем локальные ресурсы. Если где-то остались инлайн-стили,
                // временно добавь 'unsafe-inline' к style-src на время отладки.
                headers["Content-Security-Policy"] =
                    "default-src 'self'; " +
                    "script-src 'self'; " +
                    "style-src 'self'; " +
                    "img-src 'self' data:; " +
                    "font-src 'self'; " +
                    "object-src 'none'; " +
                    "base-uri 'self'; " +
                    "frame-ancestors 'none'";

                await next();
            });

            // 5) Роутинг
            app.UseRouting();

            // 6) Маршруты MVC + sitemap.xml
            app.UseEndpoints(endpoints =>
            {
                // Карта сайта (генерация на лету)
                endpoints.MapGet("/sitemap.xml", async context =>
                {
                    context.Response.ContentType = "application/xml; charset=utf-8";
                    var now = DateTime.UtcNow.ToString("yyyy-MM-dd");
                    var xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
  <url><loc>https://bluetech.az/</loc><lastmod>{now}</lastmod><changefreq>weekly</changefreq><priority>1.0</priority></url>
  <url><loc>https://bluetech.az/Services</loc><lastmod>{now}</lastmod><changefreq>monthly</changefreq><priority>0.8</priority></url>
  <url><loc>https://bluetech.az/Projects</loc><lastmod>{now}</lastmod><changefreq>weekly</changefreq><priority>0.8</priority></url>
  <url><loc>https://bluetech.az/Home/About</loc><lastmod>{now}</lastmod><changefreq>yearly</changefreq><priority>0.5</priority></url>
  <url><loc>https://bluetech.az/Home/Contact</loc><lastmod>{now}</lastmod><changefreq>yearly</changefreq><priority>0.5</priority></url>
</urlset>";
                    await context.Response.WriteAsync(xml);
                });

                // Маршрут по умолчанию
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
