using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace appui
{
    internal class PageReaderFactory : IPageReaderFactory
    {
        private readonly bool? Offline;
        private readonly IServiceProvider ServiceProvider;

        public PageReaderFactory(IOptions<AppSettings> config, IServiceProvider serviceProvider)
        {
            Offline = config?.Value?.DefaultCatalogConnector?.Offline;
            ServiceProvider = serviceProvider;
        }

        public IPageReader CreatePageReader()
        {
            switch (Offline.GetValueOrDefault())
            {
                case true:
                    return ServiceProvider?.GetRequiredService<OfflineFilePageReader>();
                case false:
                    return ServiceProvider?.GetRequiredService<WebPageReader>();
            }
        }
    }
}
