using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class ConnectionStringInfo : IConnectionStringInfo
    {
        public string Id { get; set; }
        public string Client { get; set; }
        public string Database { get; set; }
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
    }
}
