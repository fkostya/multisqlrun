using appui.connectors;
using appui.models.Interfaces;
using appui.models.Payloads;
using appui.shared;
using appui.shared.Enums;
using appui.shared.Interfaces;
using appui.shared.Models;
using appui.shared.RabbitMQ;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            services
                .AddLogging(configure => configure.AddConsole())
                .AddLogging(configure => configure.AddNLog())
                .AddTransient<MainForm>()
                .AddScoped<WebPageReader>()
                .AddScoped<OfflineFilePageReader>()
                .AddScoped<IServiceProvider, ServiceProvider>()
                .Configure<IEnumerable<ResourceCatalog>>(Configuration.GetSection("catalogSourceSettings:catalogConnections"))
                .Configure<AppSettings>(Configuration.GetSection("appSettings"))
                .Configure<SqlSettings>(Configuration.GetSection("sqlSettings"))
                .Configure<MessagingSettings>(Configuration.GetSection("messagingSettings"))
                .Configure<RabbitMqSettings>(Configuration.GetSection("rabbitmqSettings"))
                .AddSingleton<CredentialCache>()
                .AddSingleton<HtmlWeb>()
                .AddSingleton<ITenantManager, TenantManager>()
                .AddSingleton<DefaultConnectorFactory>()
                .AddSingleton<DFConnector>()
                .AddTransient<MsSqlQueryConnector>()
                .AddSingleton<RabbitMqProducer>()
                .AddSingleton<SingleThreadContext>()
                .AddTransient<IMessagePayload, MsSqlMessagePayload>()
                .AddTransient<IMessagePayload, SaveCvsFileMessagePayload>()
                .AddSingleton<IMessageProducer>((configure) =>
                {
                    return Configuration.GetSection("appSettings").Get<AppSettings>().Mode.ToLower() switch
                    {
                        "rabbitmq" => configure.GetService<RabbitMqProducer>(),
                        _ => configure.GetService<SingleThreadContext>(),
                    };
                });
        }
    }
}