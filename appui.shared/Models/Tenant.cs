using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class Tenant : ITenant
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public ITenantConnection Connection { get; set; }
    }
}
