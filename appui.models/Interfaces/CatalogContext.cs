using System.Diagnostics.CodeAnalysis;

namespace appui.models.Interfaces
{
    [ExcludeFromCodeCoverage]
    public abstract class CatalogContext
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Tenant<Connection>>? Tenants { get; set; }
    }
}