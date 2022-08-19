namespace appui.shared.Interfaces
{
    /// <summary>
    /// Class represents tenant manager layer, like load catalogs\tenants, find tenants by version\key
    /// </summary>
    public interface ITenantManager
    {
        Task<IList<ICatalog>> LoadCatalogs();

        Task<IList<ITenant>> LoadTenants(ICatalog catalog);

        IList<ITenant> FindTenants(string version, string key = "");
    }
}
