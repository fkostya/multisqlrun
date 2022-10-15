using System.Diagnostics.CodeAnalysis;

namespace appui.models
{
    [ExcludeFromCodeCoverage]
    public class ConnectorSetting
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? CatalogConnectionId { get; set; }
        public Args? Args { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Args
    {
        public string? Version { get; set; }
    }
}