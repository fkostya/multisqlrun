namespace appui.shared.Interfaces
{
    public interface ITenantConnection
    {
        string DbServer { get; set; }
        string Database { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
