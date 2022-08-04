using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace appui.shared
{
    /// <summary>
    /// The class loads connection strings from 'my' internal company web site or pre-loaded html page.
    /// </summary>
    public class DFConnector : IConnector
    {
        private readonly AppSettings appSettings;
        private readonly IServiceProvider serviceProvider;
        public bool Offline { get; set; } = true;

        public DFConnector(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            this.appSettings = appSettings.Value;
            this.serviceProvider = serviceProvider;
        }

        public IPageReader GetReader()
        {
            return this.appSettings.DefaultCatalogConnector.Offline?
                serviceProvider.GetService<OfflineFilePageReader>() :
                serviceProvider.GetService<WebPageReader>();
        }

        public async Task<IList<IConnectionStringInfo>> LoadConnectionStrings()
        {
            await Task.Delay(1);

            throw new NotImplementedException();
        }
    }
}
