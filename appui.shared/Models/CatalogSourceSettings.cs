using appui.shared.Interfaces;

namespace appui.shared.Models
{
    public class CatalogSourceSettings
    {
        public IList<ICatalog> CatalogConnections { get; set; }
    }
}
