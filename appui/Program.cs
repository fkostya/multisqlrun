using appui.connectors.Extensions;
using appui.models;
using appui.models.Interfaces;
using appui.shared;
using appui.shared.Extensions;
using appui.shared.HostedEnvironment;
using appui.shared.Interfaces;
using appui.shared.Models;
using appui.shared.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Forms;

namespace appui
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        private static IConfiguration Configuration;

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
                _ = serviceProvider.GetService<MsWindowsRunSetup>().RunSetup(services);
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

            _ = services
                .AddLogging(configure => configure.AddConsole())
                .AddLogging(configure => configure.AddNLog())
                .AddTransient<MainForm>()
                .Configure<List<ResourceCatalog>>(Configuration.GetSection("catalogSourceSettings:catalogConnections"))
                .Configure<AppSettings>(Configuration.GetSection("appSettings"))
                .Configure<SqlSettings>(Configuration.GetSection("sqlSettings"))
                .Configure<MessagingSettings>(Configuration.GetSection("messagingSettings"))
                .Configure<RabbitMqSettings>(Configuration.GetSection("rabbitmqSettings"))
                .Configure<List<ConnectorSetting>>(Configuration.GetSection("connectorSettings:connectorSetting"))
                .AddSingleton<IMessageProducer>((configure) =>
                {
                    return Configuration.GetSection("appSettings").Get<AppSettings>().Mode.ToLower() switch
                    {
                        "rabbitmq" => configure.GetService<RabbitMqProducer>(),
                        _ => configure.GetService<SingleThreadContext>(),
                    };
                })
                .AddScoped<DFConnector>((configure) =>
                {
                    IPageReader reader =
                        Configuration.GetSection("appSettings").Get<AppSettings>().Offline ?
                        configure.GetService<OfflineFilePageReader>() : configure.GetService<WebPageReader>();

                    return new DFConnector(reader);
                })
                .AddSingleton<SingleThreadContext>()
                .AddSharedServices()
                .AddConnectorsServices()
                .AddSingleton<CreateSecurityStorageFile>()
                .AddSingleton<IEnvSetupFactory, EnvSetupFactory>()
                .AddSetupHandlers<MsWindows64>((config) =>
                {
                    var factory = config.GetService<IEnvSetupFactory>();
                    factory.RegisterHandler("CreateSecurityStorageFile", config.GetService<CreateSecurityStorageFile>());
                })
                .AddSingleton<MsWindowsRunSetup>();
        }
    }
}