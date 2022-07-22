using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Windows.Forms;

namespace appui
{
    static class Program
    {
        public static IConfiguration Configuration;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var form1 = serviceProvider.GetRequiredService<MainForm>();
                Application.Run(form1);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            Configuration  = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            services
                .AddLogging(configure => configure.AddConsole())
                .AddLogging(configure => configure.AddNLog())
                .AddTransient<MainForm>()
                .AddTransient<WebPageReader>()
                .AddTransient<OfflineFilePageReader>()
                .AddScoped<IServiceProvider, ServiceProvider>()
                .Configure<ConnectionSourceOption>(Configuration.GetSection("connectionSource"))
                .Configure<AppSettings>(Configuration.GetSection("appSettings"))
                .AddSingleton<ILoadConnections, LoadConnections>()
                .AddSingleton<IPageReaderFactory, PageReaderFactory>();
        }
    }
}
