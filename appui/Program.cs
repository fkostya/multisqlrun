using appui.shared;
using appui.shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace appui
{
    static class Program
    {
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

        private static void ConfigureServices(ServiceCollection services)
        {
            services
                .AddLogging(configure => configure.AddConsole())
                .AddTransient<MainForm>();
            //.AddSingleton<ILoadConnections, LoadConnections>();

            //services.Configure<Config>(Configuration.GetSection(nameof(Config)));

            if (Config.Offline)
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
