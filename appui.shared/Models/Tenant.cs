using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class Tenant : ITenant
    {
        public string Name { get; set; }

        public ITenantConnection Connection { get; set; }
    }
}
