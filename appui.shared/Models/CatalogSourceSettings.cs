namespace appui.shared.Models
{
    public class CatalogSourceSettings
    {
        public IList<CatalogConnection> CatalogConnections { get; set; }
    }

    public class CatalogConnection
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Url { get; set; }
        public string FilePath { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public string Parse { get; set; }
    }
}
