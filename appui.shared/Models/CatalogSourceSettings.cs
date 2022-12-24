using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class CatalogSourceSettings
    {
        public IList<ITenantCatalog> CatalogConnections { get; set; }
    }
}
