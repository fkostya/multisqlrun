using appui.shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    /// <summary>
    /// Load tenants information from source in cross-tenant mode
    /// sources like file, web(in future can be extended to aws, azure, db.....)
    /// </summary>
    public class TenantManager : ITenantManager
    {
        public TenantManager()
        {

        }

        public async Task<List<ICatalog>> LoadCadalogsFromSource(IConnector connector)
        {
            var reader = connector.LoadConnectionStrings();
            return null;
        }

        public ITenant LoadTenantsFromCatalog(string catalog)
        {
            return null;
        }
    }
}
