using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
