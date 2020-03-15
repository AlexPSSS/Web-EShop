using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;
using WebStore.Infrastructure.AutoMapper;
using WebStore.Services.Product;
using WebStore.Logger;
using WebStore.Infrastructure.Middleware;
using WebStore.Hubs;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddAutoMapper(opt =>
            {
                opt.AddProfile<ViewModelMapping>();
                opt.AddProfile<DTOMapping>();
            }, typeof(Startup)/*.Assembly*/);

            // Добавляем разрешение зависимости
            services.AddScoped<IProductService, ProductsClient>();
            services.AddScoped<IOrdersService, OrdersClient>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartStore, CookiesCartStore>();

            services.AddSingleton<IEntityListService<EmployeeViewModel>, EmployeesClient>();
            services.AddScoped<IValuesService, ValuesClient>();

            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();

            #region Custom implementation identity storages
            services.AddTransient<IUserStore<User>, UsersClient>();
            services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            services.AddTransient<IUserEmailStore<User>, UsersClient>();
            services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            services.AddTransient<IUserLockoutStore<User>, UsersClient>();
            services.AddTransient<IUserClaimStore<User>, UsersClient>();
            services.AddTransient<IUserLoginStore<User>, UsersClient>();

            services.AddTransient<IRoleStore<Role>, RolesClient>();
            #endregion

            services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = false;
                }
            );

            // 4FU
            //services.ConfigureApplicationCookie(options => // необязательно
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.Expiration = TimeSpan.FromDays(150);
            //    //options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
            //    //options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
            //    //options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
            //    options.SlidingExpiration = true;
            //});

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(SimpleActionFilter)); // подключение по типу
            //    //альтернативный вариант подключения
            //    //options.Filters.Add(new SimpleActionFilter()); // подключение по объекту
            //});
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //app.UseWelcomePage("/welcome");

            //app.Map("/index", CustomIndexHandler);

            //app.UseMiddleware<TokenMiddleware>();

            //UseSample(app);

            //var helloMessage = _configuration["CustomHelloWorld"];
            //var logLevel = _configuration["Logging:LogLevel:Microsoft"];
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<InformationHub>("/info");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                // Маршрут по умолчанию состоит из трёх частей разделённых “/”
                // Первой частью указывается имя контроллера,
                // второй - имя действия (метода) в контроллере,
                // третей - опциональный параметр с именем “id”
                // Если часть не указана - используются значения по умолчанию:
                // для контроллера имя “Goods”,
                // для действия - “Index”

                //endpoints.Map("/hello", async context =>
                //{
                //    await context.Response.WriteAsync(helloMessage);
                //});
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Даже не знаю, что Вам сказать...");
            //});
        }

        //private void UseSample(IApplicationBuilder app)
        //{
        //    app.Use(async (context, next) =>
        //    {
        //        bool isError = false;
        //        // ...
        //        if (isError)
        //        {
        //            await context.Response
        //                .WriteAsync("Error occured. You're in custom pipeline module...");
        //        }
        //        else
        //        {
        //            await next.Invoke();
        //        }
        //    });
        //}

        //private void CustomIndexHandler(IApplicationBuilder app)
        //{
        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Hello from custom /Index handler");
        //    });
        //}
    }
}
