using appui.models;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Net;

namespace appui.shared
{
    /// <summary>
    /// Load tenants information from source in cross-tenant mode
    /// sources like file, web(in future can be extended to aws, azure, db.....)
    /// </summary>
    public class TenantManager : ITenantManager
    {
        private IEnumerable<ResourceCatalog> catalogs;
        private readonly CatalogSourceDownloadFactory catalogSourceDownloadFactory;
        private readonly IServiceProvider serviceProvider;

        public TenantManager(IOptions<List<ResourceCatalog>> catalogs, CatalogSourceDownloadFactory catalogSourceDownloadFactory, IServiceProvider serviceProvider)
        {
            this.catalogSourceDownloadFactory = catalogSourceDownloadFactory;
            this.catalogs = catalogs.Value.AsEnumerable<ResourceCatalog>();
            this.serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<ResourceCatalog>> LoadCatalogs()
        {
            return await Task.FromResult(catalogs);
        }

        public async Task<IEnumerable<ITenant>> LoadTenants(ConnectorSetting catalog)
        {
            if (catalog == null) return await Task.FromResult(new List<ITenant>());

            var loadFromCatalog = this.catalogs.FirstOrDefault(f => f.Id.Equals(catalog.CatalogConnectionId));
            if (loadFromCatalog == null) return await Task.FromResult(new List<ITenant>());

            var download = catalogSourceDownloadFactory.CreateReaderFactory(loadFromCatalog.Type);
            if (download == null) return await Task.FromResult(new List<ITenant>());
            var connector = ActivatorUtilities.CreateInstance<DFConnector>(serviceProvider, (IPageReader)download);

            var dict = new Dictionary<string, object>() { { "version", catalog.Args?.Version } };

            var connectionStrings = await connector.LoadConnectionStrings(dict);
            var tenants = new List<ITenant>();
            foreach (var cs in connectionStrings)
            {
                tenants.Add(new Tenant
                {
                    Name = cs.Client,
                    Version = cs.Version,
                    Connection = new TenantConnection
                    {
                        Database = cs.Database,
                        UserName = cs.UserName,
                        Password = cs.Password
                    }
                });
            }

            return tenants;
        }
    }
}
