using appui.shared.Interfaces;
using HtmlAgilityPack;

namespace appui.shared
{
    public class LoadConnections : ILoadConnections
    {
        private readonly IPageReader reader;

        public LoadConnections(IPageReader reader)
        {
            this.reader = reader;
        }

        public async Task<IList<IConnectionRecord>> Load()
        {
            var htmlDoc = await this.reader.GetPageAsync();

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