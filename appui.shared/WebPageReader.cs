using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace appui.shared
{
    public class WebPageReader : IPageReader
    {
        private readonly Catalog config;
        private readonly NetworkCredential credential;
        private readonly HtmlWeb web;
        private const string support_type = "web-url";
        private readonly Catalog defaultConnection = new Catalog();
        private readonly HtmlDocument defaultHtmlDocument = new HtmlDocument();
        private readonly ILogger logger;

        public WebPageReader(IOptions<List<Catalog>> options, CredentialCache credentialCache, HtmlWeb htmlWeb, ILogger<AppErrorLog> logger)
        {
            this.logger = logger;
            this.config = options?.Value?
                .Where(f => f.Type== support_type)
                .Cast<Catalog>()
                .SingleOrDefault();

            if (config == null)
            {
                config = defaultConnection;
                logger.LogInformation("Connector is not provided, using an empty connector");
            }

            if (!string.IsNullOrEmpty(config.Url))
            {
                this.credential = credentialCache.GetCredential(new Uri(this.config?.Url), "Basic");
            }
            logger.LogInformation($"WebPageReader uses url={config.Url}");

            this.web = htmlWeb;
        }

        public async Task<HtmlDocument> LoadPageAsync()
        {
            HtmlDocument doc = null;
            try
            {
                doc = await web.LoadFromWebAsync(config.Url, credential);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                doc = defaultHtmlDocument;
            }

            return doc;
        }
    }
}
