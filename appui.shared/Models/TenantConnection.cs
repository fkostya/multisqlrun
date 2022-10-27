using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class TenantConnection : ITenantConnection
    {
        public string DbServer { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
