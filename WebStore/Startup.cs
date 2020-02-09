using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL;
using WebStore.DomainNew.Entities;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Implementations;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(SimpleActionFilter)); // подключение по типу
                //альтернативный вариант подключени€
                //options.Filters.Add(new SimpleActionFilter()); // подключение по объекту

            });

            services.AddDbContext<WebStoreContext>(options => options
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            // ƒобавл€ем разрешение зависимости
            services.AddSingleton<IEntityListService<EmployeeViewModel>, InMemoryEmployeesService>();
            //services.AddTransient<IEntityListService, InMemoryEmployeesService>();
            //services.AddScoped<IEntityListService, InMemoryEmployeesService>();
            services.AddSingleton<IEntityListService<GoodsView>, InMemoryGoodsService>();

            //services.AddSingleton<IProductService, InMemoryProductService>();
            // SQL now!
            services.AddScoped<IProductService, SqlProductService>();
            services.AddScoped<IOrdersService, SqlOrdersService>();


            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;
                }
            );

            // 4FU
            //services.ConfigureApplicationCookie(options => // необ€зательно
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.Expiration = TimeSpan.FromDays(150);
            //    //options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
            //    //options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
            //    //options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
            //    options.SlidingExpiration = true;
            //});

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CookieCartService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseWelcomePage("/welcome");

            app.Map("/index", CustomIndexHandler);

            app.UseMiddleware<TokenMiddleware>();

            UseSample(app);

            var helloMessage = _configuration["CustomHelloWorld"];
            var logLevel = _configuration["Logging:LogLevel:Microsoft"];

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                // ћаршрут по умолчанию состоит из трЄх частей разделЄнных У/Ф
                // ѕервой частью указываетс€ им€ контроллера,
                // второй - им€ действи€ (метода) в контроллере,
                // третей - опциональный параметр с именем УidФ
                // ≈сли часть не указана - используютс€ значени€ по умолчанию:
                // дл€ контроллера им€ УGoodsФ,
                // дл€ действи€ - УIndexФ

                endpoints.Map("/hello", async context =>
                {
                    await context.Response.WriteAsync(helloMessage);
                });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("ƒаже не знаю, что ¬ам сказать...");
            });
        }

        private void UseSample(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                bool isError = false;
                // ...
                if (isError)
                {
                    await context.Response
                        .WriteAsync("Error occured. You're in custom pipeline module...");
                }
                else
                {
                    await next.Invoke();
                }
            });
        }

        private void CustomIndexHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from custom /Index handler");
            });
        }
    }
}
