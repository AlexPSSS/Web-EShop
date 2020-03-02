using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using WebStore.DAL;

namespace WebStore
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        var host = BuildWebHost(args);

    //        using (var scope = host.Services.CreateScope()) // нужно для получения DbContext
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

    public class Program
    {
        public static void Main(string[] args) =>
            CreateWebHostBuilder(args)
                .Build()
                .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureLogging((host, log) =>
                // {
                //     log.AddFilter("Microsoft", level => level > LogLevel.Information);
                //     log.ClearProviders();
                //     log.AddConsole(opt => opt.IncludeScopes = true);
                //     log.AddDebug();
                // })
                //.UseUrls("http://0.0.0.0:8080")
                .UseStartup<Startup>()
                .UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
                   .MinimumLevel.Debug()
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                   .Enrich.FromLogContext()
                   .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                   .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
                   .WriteTo.File(new JsonFormatter(",", true), $@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
                   .WriteTo.Seq("http://localhost:5341/"));
    }

}
