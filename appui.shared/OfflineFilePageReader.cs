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
                doc.Load(this.getFilePath(config?.FilePath));
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, ex.Message);

                doc = new HtmlDocument();
            }
            return await Task.FromResult(doc);
        }

        private string getFilePath(string path)
        {
            var env_variable = '$';
            var path_splitter = "\\";
            if (!string.IsNullOrEmpty(path))
            {
                var path_split = path.Split(path_splitter);
                var new_path = new List<string>();
                for (var i = 0; i < path_split.Length; i++)
                {
                    new_path.Add(path_split[i][0] == env_variable ?
                        Environment.GetEnvironmentVariable(path_split[i].TrimStart('$')).TrimStart('\\').TrimStart('\\').TrimEnd('\\').TrimEnd('\\')
                        : path_split[i]);
                }
                return new_path.Aggregate((a, b) => a + path_splitter + b);
            }
            return path;
        }
    }
}
