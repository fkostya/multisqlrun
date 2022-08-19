﻿using appui.shared.Interfaces;
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
        private IList<ICatalog> catalogs;

        public TenantManager(IOptions<List<ICatalog>> catalogs, DefaultConnectorFactory defaultConnectorFactory)
        {
            this.connector = defaultConnectorFactory.GetConnectorFactory();

            this.catalogs = catalogs.Value;
        }

        public async Task<IList<ICatalog>> LoadCatalogs()
        {
            return await Task.FromResult(catalogs);
        }

        public async Task<IList<ITenant>> LoadTenants(ICatalog catalog)
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

        public IList<ITenant> FindTenants(string tenantVersion, string tenantName = "")
        {
            if (string.IsNullOrEmpty(tenantVersion) || !tenants.Any(f => f.Version.Equals(tenantVersion, StringComparison.CurrentCultureIgnoreCase)))
                return new List<ITenant>();

            return tenants
                .Where(f => f.Version.Equals(tenantVersion, StringComparison.CurrentCultureIgnoreCase))
                .Where(f => f.Name.Contains(tenantName, StringComparison.OrdinalIgnoreCase) || f.Connection.Database.Contains(tenantName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
