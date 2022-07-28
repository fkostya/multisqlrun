using appui.shared.Interfaces;

namespace appui.shared
{
    public class LoadConnections : ILoadConnections
    {
        private readonly IPageReader reader;
        private Dictionary<string, List<IConnectionRecord>> connectionIndex;

        public LoadConnections(IPageReaderFactory pageReaderFactory)
        {
            this.reader = pageReaderFactory.CreatePageReader();
        }

        public async Task<IList<IConnectionRecord>> Load()
        {
            connectionIndex = new Dictionary<string, List<IConnectionRecord>>();

            var htmlDoc = await this.reader.GetPageAsync();

            var doc = new WebDocument(htmlDoc);

            IEnumerable<IConnectionRecord> sites = doc.GetConnections();
            var connections = new List<IConnectionRecord>();
            foreach (IConnectionRecord site in sites)
            {
                if (!connectionIndex.ContainsKey(site.Version))
                {
                    connectionIndex.Add(site.Version, new List<IConnectionRecord>());
                }
                connectionIndex[site.Version].Add(site);
                connections.Add(site);
            }

            return connections;
        }

        public IList<IConnectionRecord> Find(string version, string key = "")
        {
            if (string.IsNullOrEmpty(version) || !connectionIndex.ContainsKey(version)) return new List<IConnectionRecord>();

            return connectionIndex[version]
                .Where(f => f.client.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                f.database.Contains(key, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}