using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Implementations;
using WebStore.Infrastructure.Interfaces;

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

            // ƒобавл€ем разрешение зависимости
            services.AddSingleton<IEmployeesService, InMemoryEmployeesService>();
            //services.AddTransient<IEmployeesService, InMemoryEmployeesService>();
            //services.AddScoped<IEmployeesService, InMemoryEmployeesService>();

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

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
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
