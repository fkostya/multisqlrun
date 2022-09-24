using appui.shared.Models;

namespace appui.shared.Interfaces
{
    public interface ICatalog
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Parser Parse { get; set; }
        public string ConnectionString { get; set; }
        public string FilePath { get; set; }
        public string Url { get; set; }
    }
}
