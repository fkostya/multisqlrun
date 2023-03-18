using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Models
{
    [ExcludeFromCodeCoverage]
    public class SqlSettings
    {
        public int ConnectionTimeout { get; set; }

        public Credential Credential { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Credential
    {
        public string Password { get; set; }
        public string UserId { get; set; }
        public bool IntegratedSecurity { get; set; }
    }
}
