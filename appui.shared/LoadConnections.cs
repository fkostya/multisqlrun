using appui.shared.Interfaces;

namespace appui.shared
{
    /// <summary>
    /// Database connections storage class.
    /// Load connections from sigle HTML file with server, database, user name and password or 
    /// load HTML page from server.
    /// </summary>
    public class LoadConnections : ILoadConnections
    {
        private readonly IPageReader reader;
        private Dictionary<string, List<IConnectionStringInfo>> connectionIndex;

        public LoadConnections(IPageReaderFactory pageReaderFactory)
        {
            this.reader = pageReaderFactory.CreatePageReader();
        }

        public async Task<IList<IConnectionStringInfo>> Load()
        {
            connectionIndex = new Dictionary<string, List<IConnectionStringInfo>>();

            var htmlDoc = await this.reader.GetPageAsync();

            var doc = new WebDocument(htmlDoc);

            IEnumerable<IConnectionStringInfo> sites = doc.GetConnections();
            var connections = new List<IConnectionStringInfo>();
            foreach (IConnectionStringInfo site in sites)
            {
                if (!connectionIndex.ContainsKey(site.Version))
                {
                    connectionIndex.Add(site.Version, new List<IConnectionStringInfo>());
                }
                connectionIndex[site.Version].Add(site);
                connections.Add(site);
            }

            return connections;
        }

        public IList<IConnectionStringInfo> Find(string version, string key = "")
        {
            if (string.IsNullOrEmpty(version) || !connectionIndex.ContainsKey(version)) return new List<IConnectionStringInfo>();

            return connectionIndex[version]
                .Where(f => f.Client.Contains(key, StringComparison.OrdinalIgnoreCase) ||
                f.Database.Contains(key, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}