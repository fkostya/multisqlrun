namespace appui.shared.Models
{
    public class CatalogSourceSettings
    {
        public IList<CatalogConnection> CatalogConnections { get; set; }
    }

    public class CatalogConnection
    {
        public string Type { get; set; }
        public string ConnectionString { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public string Format { get; set; }
    }
}
