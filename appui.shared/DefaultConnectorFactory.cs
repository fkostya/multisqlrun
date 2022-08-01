using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    public class DefaultConnectorFactory
    {
        private readonly ILogger _logger;
        private readonly AppSettings _settings;

        public DefaultConnectorFactory()
        {

        }

        public DefaultConnectorFactory(IOptions<AppSettings> appSettings, ILogger<AppErrorLog> logger)
        {
            _settings = appSettings.Value;
            _logger = logger;
        }

        public IConnector CreateDefaultConnector()
        {
            try
            {
                var assemblyFile = $"{Environment.CurrentDirectory}\\{_settings.DefaultCatalogConnector.AssemblyFile}";
                var typeName = _settings.DefaultCatalogConnector.TypeName;
                _logger.LogDebug($"Creating instance of: {assemblyFile}.{typeName}");

                Assembly assembly = Assembly.LoadFrom(assemblyFile);

                Type type = assembly.GetType(typeName);
                IConnector connector = (IConnector)Activator.CreateInstance(type);
                return connector;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new EmptyConnector();
        }
    }
}
