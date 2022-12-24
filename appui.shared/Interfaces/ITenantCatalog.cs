using appui.shared.Models;

namespace appui.shared.Interfaces
{
    public interface ITenantCatalog
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Parser Parse { get; set; }
        public string Connection { get; set; }
        public IEnumerable<ITenant> Tenants { get; set; }
    }
}
