using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace appui.shared
{
    public class OfflineFilePageReader : IPageReader
    {
        private readonly ResourceCatalog config;
        private const string support_type = "df-windows-file";
        private readonly ILogger logger;

        public OfflineFilePageReader(IOptions<List<ResourceCatalog>> options, ILogger<AppErrorLog> logger)
        {
            this.config = options?.Value?
               .Where(f => f.Type.Equals(support_type, StringComparison.OrdinalIgnoreCase))
               .Cast<ResourceCatalog>()
               .FirstOrDefault();

            this.logger = logger;

            this.logger?.LogInformation($"OfflineFilePageReader will load file from path: {this.config?.FilePath}");
        }

        public async Task<HtmlDocument> LoadPageAsync()
        {
            var doc = new HtmlDocument();
            try
            {
                doc.Load(config?.FilePath);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, ex.Message);

                doc = new HtmlDocument();
            }
            return await Task.FromResult(doc);
        }
    }
}
