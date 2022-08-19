using appui.shared.Interfaces;

namespace appui.shared.Models
{
    public abstract class Catalog : ICatalog
    {
        public string Name { get; set; }
        public string Type { get; set; }
        
        public Parser Parse { get; set; }
    }

    class SqlCatalog : Catalog
    {
        public string ConnectionString { get; set; }
    }

    public class FileCatalog: Catalog
    {
        public string FilePath { get; set; }
    }

    public class WebCatalog : Catalog
    {
        public string Url { get; set; }
    }

    public class Parser
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Field { get; set; }
    }
}
