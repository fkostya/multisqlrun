using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appui
{
    public class OfflineFilePageReader : IPageReader
    {
        private readonly CatalogConnection config;
        private const string support_type = "df-offline";


        public OfflineFilePageReader(IOptions<List<CatalogConnection>> options)
        {
            config = options?.Value?
               .Where(f => f.Name == support_type)
               .FirstOrDefault();
        }

        public async Task<HtmlDocument> GetPageAsync()
        {
            var doc = new HtmlDocument();
            doc.Load(config.FilePath);

            return await Task.FromResult(doc);
        }
    }
}
