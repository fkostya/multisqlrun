using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
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
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            Configuration = builder.Build();

            services
                .AddLogging(configure => configure.AddConsole())
                .AddTransient<MainForm>()
                .Configure<ConnectionSourceOption>(Configuration.GetSection("connectionSource"))
                .Configure<GeneralOption>(Configuration.GetSection("general"));
            
            //.AddSingleton<ILoadConnections, LoadConnections>();

            if (true)
            {
                services.AddScoped<IPageReader, OfflineFilePageReader>();
            }
            else
            {
                services.AddScoped<IPageReader, WebPageReader>();
            }
        }
    }
}
