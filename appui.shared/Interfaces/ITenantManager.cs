namespace appui.shared.Interfaces
{
    /// <summary>
    /// Class represents tenant manager layer, like load tenants, find tenants by version\key
    /// </summary>
    public interface ITenantManager
    {
        Task<IList<ITenant>> LoadTenantsFromCatalog();
        IList<ITenant> Find(string version, string key = "");
    }
}
