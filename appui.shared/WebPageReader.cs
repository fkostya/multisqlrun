using appui.shared.Interfaces;
using appui.shared.Models;
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
        private readonly ConnectionSourceOption config;

        public WebPageReader(IOptions<ConnectionSourceOption> options)
        {
            this.config = options.Value;
        }
        public async Task<HtmlDocument> GetPageAsync()
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(config.WebConnectionSource);

            return doc;
        }
    }
}
