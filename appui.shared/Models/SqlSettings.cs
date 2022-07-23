using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Models
{
    public class SqlSettings
    {
        public int ConnectionTimeout { get; set; }

        public Credential Credential { get; set; }
    }

    public class Credential
    {
        public string Password { get; set; }
        public string UserId { get; set; }
        public bool IntegratedSecurity { get; set; }
    }
}
