using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Net;

namespace appui.shared
{
    public class WebPageReader : IPageReader
    {
        private readonly ConnectionSourceOption config;
        private readonly NetworkCredential credential;
        private readonly HtmlWeb web;

        public WebPageReader(IOptions<ConnectionSourceOption> options, CredentialCache credentialCache, HtmlWeb htmlWeb)
        {
            config = options?.Value;
            credential = credentialCache.GetCredential(new Uri(config.WebConnectionSource), "Basic"); ;
            web = htmlWeb;
        }

        public async Task<HtmlDocument> GetPageAsync()
        {
            var doc = await web.LoadFromWebAsync(config.WebConnectionSource, credential);

            return doc;
        }
    }
}
