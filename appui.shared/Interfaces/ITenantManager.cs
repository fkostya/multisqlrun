using appui.models;
using appui.shared.Models;

namespace appui.shared.Interfaces
{
    /// <summary>
    /// Class represents tenant manager layer, like load catalogs\tenants, find tenants by version\key
    /// </summary>
    public interface ITenantManager
    {
        Task<IEnumerable<ResourceCatalog>> LoadCatalogs();

        Task<IList<ITenant>> LoadTenants(ConnectorSetting catalog);
    }
}
