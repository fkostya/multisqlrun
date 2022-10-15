using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace appui.shared
{
    public class ConnectorFactory
    {
        private readonly IServiceProvider serviceProvider;
        public ConnectorFactory(IServiceProvider _serviceProvider)
        {
            this.serviceProvider = _serviceProvider;
        }

        public IConnector CreateConnector(string type)
        {
            return type switch
            {
                "df-web-url" => ActivatorUtilities.CreateInstance<IConnector>(this.serviceProvider, typeof(WebPageReader)),
                "df-windows-file" => ActivatorUtilities.CreateInstance<IConnector>(this.serviceProvider, typeof(OfflineFilePageReader)),
                _ => null,
            };
        }
    }
}