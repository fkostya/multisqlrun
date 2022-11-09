namespace appui.shared.Interfaces
{
    public interface ITenant
    {
        string Name { get; set; }
        ITenantConnection Connection { get; set; }
    }
}
