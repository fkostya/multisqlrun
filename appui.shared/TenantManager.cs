using appui.shared.Interfaces;
using appui.shared.Models;

namespace appui.shared
{
    /// <summary>
    /// Load tenants information from source in cross-tenant mode
    /// sources like file, web(in future can be extended to aws, azure, db.....)
    /// </summary>
    public class TenantManager : ITenantManager
    {
        private readonly IConnector connector;
        private Dictionary<string, List<ITenant>> tenantIndex;

        public TenantManager(DefaultConnectorFactory defaultConnectorFactory)
        {
            this.connector = defaultConnectorFactory.GetConnectorFactory();
        }

        public async Task<IList<ITenant>> LoadTenantsFromCatalog()
        {
            var connectionStrings = await connector.LoadConnectionStrings();
            var tenants = new List<ITenant>();
            foreach (var cs in connectionStrings)
            {
                tenants.Add(new Tenant { 
                    Name = cs.Client,
                    Version = cs.Version,
                    ConnectionString = new TenantConnectionString
                    {
                        Database = cs.Database,
                        UserName = cs.UserName,
                        Password = cs.Password
                    }
                });
            }

            return tenants;
        }

        public IList<ITenant> Find(string version, string key = "")
        {
            return null;
            //if (string.IsNullOrEmpty(version) || !connectionIndex.ContainsKey(version)) return new List<IConnectionStringInfo>();

            //return connectionIndex[version]
            //    .Where(f => f.Client.Contains(key, StringComparison.OrdinalIgnoreCase) ||
            //    f.Database.Contains(key, StringComparison.OrdinalIgnoreCase))
            //    .ToList();
        }
    }
}
