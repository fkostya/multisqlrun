using appui.shared.Interfaces;
using System.Security;

namespace appui.shared.Models
{
    public class Tenant : ITenant
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public ITenantConnectionString ConnectionString { get; set; }
    }
}
