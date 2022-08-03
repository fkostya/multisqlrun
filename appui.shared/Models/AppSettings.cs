using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared.Models
{
    public class AppSettings
    {
        public int StopAfterMilliseconds { get; set; }
        public string OutputFolder { get; set; }
        public DefaultCatalogConnector DefaultCatalogConnector { get; set; }
    }

    public class DefaultCatalogConnector
    {
        public bool Offline { get; set; }
        public string TypeName { get; set; }
        public string AssemblyFile { get; set; }
    }
}
