using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class ResourceCatalog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Parser Parse { get; set; }
        public string ConnectionString { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Parser
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Field { get; set; }
    }
}
