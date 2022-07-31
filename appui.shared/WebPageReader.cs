using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Net;

namespace appui.shared
{
    public class WebPageReader : IPageReader
    {
        private readonly CatalogConnection config;
        private readonly NetworkCredential credential;
        private readonly HtmlWeb web;
        private const string support_type = "web";

        public WebPageReader(IOptions<List<CatalogConnection>> options, CredentialCache credentialCache, HtmlWeb htmlWeb)
        {
            config = options?.Value?
                .Where(f => f.Type == support_type)
                .FirstOrDefault();

            credential = credentialCache.GetCredential(new Uri(this.config?.ConnectionString), "Basic");
            web = htmlWeb;
        }

        public async Task<HtmlDocument> GetPageAsync()
        {
            var doc = await web.LoadFromWebAsync(config.ConnectionString, credential);

            return doc;
        }
    }
}
