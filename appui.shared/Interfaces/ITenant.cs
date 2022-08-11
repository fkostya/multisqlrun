namespace appui.shared.Interfaces
{
    public interface ITenant
    {
        string Name { get; set; }
        string Version { get; set; }
        ITenantConnection Connection { get; set; }
    }
}
