using appui.shared.Interfaces;
using HtmlAgilityPack;

namespace appui.shared
{
    public class LoadConnections : ILoadConnections
    {
        private readonly string url;
        private readonly IPageReader reader;

        public LoadConnections(IPageReader reader, string url)
        {
            this.reader = reader;
            this.url = url;
        }

        public async Task<IList<IConnectionRecord>> Load()
        {
            var htmlDoc = await this.reader.GetPageAsync(this.url);

            var list = new List<IConnectionRecord>();
            var doc = new WebDocument(htmlDoc);

            IEnumerable<IConnectionRecord> sites = doc.GetConnections();
            foreach (IConnectionRecord site in sites)
            {
                list.Add(site);
            }

            return list;
        }
    }
}