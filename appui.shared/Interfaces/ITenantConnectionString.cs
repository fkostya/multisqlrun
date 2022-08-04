using System.Security;

namespace appui.shared.Interfaces
{
    public interface ITenantConnectionString
    {
        string DbServer { get; set; }
        string Database { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
