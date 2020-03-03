using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebStore.DAL;

namespace WebStore.ServiceHosting
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateWebHostBuilder(args)
                .Build()
                .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        var host = BuildWebHost(args);

    //        using (var scope = host.Services.CreateScope()) // ����� ��� ��������� DbContext
    //        {
    //            var services = scope.ServiceProvider;
    //            try
    //            {
    //                WebStoreContext context = services.GetRequiredService<WebStoreContext>();
    //                DbInitializer.Initialize(context);
    //                DbInitializer.InitializeUsers(services);
    //            }
    //            catch (Exception ex)
    //            {
    //                var logger = services.GetRequiredService<ILogger<Program>>();
    //                logger.LogError(ex, "Oops. Something went wrong at DB initializing...");
    //            }
    //        }

    //        host.Run();
    //    }

    //    private static IWebHost BuildWebHost(string[] args) =>
    //        WebHost.CreateDefaultBuilder(args)
    //            .UseStartup<Startup>()
    //            .Build();

    //}

}
