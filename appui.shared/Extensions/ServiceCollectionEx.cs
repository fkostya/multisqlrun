using appui.models.Payloads;
using appui.shared.Interfaces;
using appui.shared.Interfaces.Repositories;
using appui.shared.RabbitMQ;
using appui.shared.Repositories;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace appui.shared.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection service)
        {
            return service
                .AddScoped<WebPageReader>()
                .AddScoped<OfflineFilePageReader>()
                .AddScoped<IServiceProvider, ServiceProvider>()
                .AddSingleton<CredentialCache>()
                .AddSingleton<HtmlWeb>()
                .AddSingleton<ITenantManager, TenantManager>()
                .AddSingleton<DefaultConnectorFactory>()
                .AddSingleton<RabbitMqProducer>()
                .AddTransient<MsSqlMessagePayload>()
                .AddTransient<SaveCvsFileMessagePayload>()
                .AddSingleton<IDirectoryWrapper, DirectoryWrapper>()
                .AddSingleton<IStorageUtility, FileUtility>()
                .AddSingleton<IConnectorSettingsRepository, ConnectorSettingsAppSettingRepository>()
                .AddSingleton<ConnectorFactory>()
                .AddSingleton<CatalogSourceDownloadFactory>();
        }
    }
}