using System.Diagnostics;
using System.IO;
using Asg.Services.ApplicationFramework.Presentation.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewShore.Persistence;
using NLog.LayoutRenderers;
using NLog.Web;

namespace NewShore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var host = CreateHostBuilder(args).Build()
                    .UseIf(configuration.GetValue<bool>("Database:AutomaticMigrations"), host =>
                    {
                        return host.MigrateDataBase<NewShoreDbContext>();
                    })
;
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("NewShore has started!");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging =>
                {
                    LayoutRenderer.Register("diagnostics-activity-id", logEvent => Activity.Current?.Id);
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog(); // NLog: Setup NLog for Dependency injection
        }
    }
}