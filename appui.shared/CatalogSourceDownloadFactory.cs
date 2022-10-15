using appui.shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace appui.shared
{
    public class CatalogSourceDownloadFactory
    {
        private readonly IServiceProvider serviceProvider;
        public CatalogSourceDownloadFactory(IServiceProvider _serviceProvider)
        {
            this.serviceProvider = _serviceProvider;
        }

        public IPageReader CreateReaderFactory(string type)
        {
            return type switch
            {
                "df-web-url" => serviceProvider.GetService<WebPageReader>(),
                "df-windows-file" => serviceProvider.GetService<OfflineFilePageReader>(),
                _ => null,
            };
        }
    }
}