using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public int StopAfterMilliseconds { get; set; }
        public string OutputFolder { get; set; }
        public string Mode { get; set; }//AppMode: rabbitmq, simple
        public DefaultCatalogConnector DefaultCatalogConnector { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DefaultCatalogConnector
    {
        public bool Offline { get; set; }
        public string TypeName { get; set; }
        public string AssemblyFile { get; set; }
    }
}
