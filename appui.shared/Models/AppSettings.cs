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
        public bool Offline { get; set; }
        public DefaultCatalogConnector DefaultCatalogConnector { get; set; }
    }

    public class DefaultCatalogConnector
    {
        public string Class { get; set; }
        public string Namespace { get; set; }
        public string Interface { get; set; }
    }
}
