using appui.shared.Interfaces;

namespace appui.shared.Models
{
    public class ResourceCatalog
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Parser Parse { get; set; }
        public string ConnectionString { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }
        public string Args { get; set; }
    }

    public class Parser
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Field { get; set; }
    }
}
