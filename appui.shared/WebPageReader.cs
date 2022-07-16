using appui.shared.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    public class WebPageReader : IPageReader
    {
        private readonly string _url;

        public WebPageReader()
        {

        }
        public async Task<HtmlDocument> GetPageAsync(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            return doc;
        }
    }
}
