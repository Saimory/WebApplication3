using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApplication3.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace WebApplication3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление контекста для Identity
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Добавление контекста 
            services.AddDbContext<ModelContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
            });
             

            // Добавление поддержки MVC
            services.AddControllersWithViews();

            // Настройка логирования
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            // Конфигурация настроек куков
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always; // Для работы через HTTPS
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Проверка, находится ли приложение в режиме разработки
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Отображение детальной информации об ошибках
            }
            else
            { 
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); 
            }

            // Принудительное использование HTTPS
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Настройка маршрутов
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "account",
                    pattern: "Account/{action=Login}/{id?}",
                    defaults: new { controller = "Account" }); 
                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "Account/Register",
                    defaults: new { controller = "Account", action = "Register" });
            });
        }
    }
}
