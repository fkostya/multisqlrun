using appui.shared.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    public class PageParser : IPageParser
    {
        public IPageReader reader;

        public PageParser(IPageReader reader)
        {
            this.reader = reader;
        }

        public async Task<IList<IPageRow>> Parse(string url)
        {
            var htmlDoc = await this.reader.GetPageAsync(url);

            var list = new List<IPageRow>();
            var doc = new WebDocument(htmlDoc);

            IEnumerable<IPageRow> sites = doc.GetSites();
            foreach (IPageRow site in sites)
            {
                list.Add(site);
            }

            return list;
        }
    }
}
