using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Options;

namespace appui.shared
{
    /// <summary>
    /// Load tenants information from source in cross-tenant mode
    /// sources like file, web(in future can be extended to aws, azure, db.....)
    /// </summary>
    public class TenantManager : ITenantManager
    {
        private readonly IConnector connector;
        private IList<ITenant> tenants;
        private IEnumerable<ResourceCatalog> catalogs;

        public TenantManager(IOptions<List<ResourceCatalog>> catalogs, DefaultConnectorFactory defaultConnectorFactory)
        {
            this.connector = defaultConnectorFactory.GetConnectorFactory();
            this.catalogs = catalogs.Value.AsEnumerable<ResourceCatalog>();
        }

        public async Task<IEnumerable<ResourceCatalog>> LoadCatalogs()
        {
            return await Task.FromResult(catalogs);
        }

        public async Task<IList<ITenant>> LoadTenants(ResourceCatalog catalog)
        {
            var connectionStrings = await connector.LoadConnectionStrings();
            this.tenants = new List<ITenant>();
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


        public IList<ITenant> FindTenants(ResourceCatalog catalog, string tenantName = "")
        {
            if (catalog == null || this.tenants == null)
                return new List<ITenant>();

            return this.tenants
                .Where(f => f.Name.Contains(tenantName, StringComparison.OrdinalIgnoreCase) || f.Connection.Database.Contains(tenantName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
