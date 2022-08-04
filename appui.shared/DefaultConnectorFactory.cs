using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace appui.shared
{
    public class DefaultConnectorFactory
    {
        private readonly ILogger _logger;
        private readonly AppSettings _settings;
        private readonly IServiceProvider _config;

        public DefaultConnectorFactory(IOptions<AppSettings> appSettings, ILogger<AppErrorLog> logger, IServiceProvider configuration)
        {
            _settings = appSettings.Value;
            _logger = logger;
            _config = configuration;
        }

        public IConnector GetConnectorFactory()
        {
            try
            {
                var assemblyFile = $"{Environment.CurrentDirectory}\\{_settings.DefaultCatalogConnector.AssemblyFile}";
                var typeName = _settings.DefaultCatalogConnector.TypeName;
                _logger.LogDebug(message: $"Creating instance of: {assemblyFile}.{typeName}");

                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                Type type = assembly.GetType(typeName);
                var connector = _config?.GetService(type) as IConnector;

                _logger.LogDebug(message: $"Created instance of: {connector}");

                return connector;
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: ex.Message);
            }
            finally
            {

            }
            _logger.LogDebug(message: $"Created instance of EmptyConnector");
            return new EmptyConnector();
        }
    }
}
