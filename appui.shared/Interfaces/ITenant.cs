namespace appui.shared.Interfaces
{
    public interface ITenant
    {
        string Name { get; set; }
        string Version { get; set; }
        ITenantConnectionString ConnectionString { get; set; }
    }
}
