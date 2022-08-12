using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace appui.shared
{
    public class OfflineFilePageReader : IPageReader
    {
        private readonly CatalogConnection config;
        private const string support_type = "df-offline";
        private readonly ILogger logger;

        public OfflineFilePageReader(IOptions<List<CatalogConnection>> options, ILogger<AppErrorLog> logger)
        {
            this.config = options?.Value?
               .Where(f => f.Name == support_type)
               .FirstOrDefault();
            this.logger = logger;

            this.logger.LogInformation($"OfflineFilePageReader will load file from path: {this.config?.FilePath}");
        }

        public async Task<HtmlDocument> LoadPageAsync()
        {
            var doc = new HtmlDocument();
            try
            {
                doc.Load(config.FilePath);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);

                doc = new HtmlDocument();
            }
            return await Task.FromResult(doc);
        }
    }
}
