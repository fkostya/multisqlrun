using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace appui.shared.HostedEnvironment
{
    public class CreateSecurityStorageFile : IEnvSetupHandler
    {
        private readonly string fileName = "config.json";
        private readonly ILogger<CreateSecurityStorageFile> logger;

        public CreateSecurityStorageFile(IOptions<AppSettings> appsetings, ILogger<CreateSecurityStorageFile> logger)
        {
            this.logger = logger;
            this.fileName = appsetings?.Value?.JsonConfigFileName ?? fileName;
        }

        public async Task Execute(IHostedEnvironment hosted)
        {
            try
            {
                await hosted?.Execute(fileName);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, ex.Message);
            }
        }

        public string GetConfigFileName()
        {
            return fileName;
        }
    }
}