using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    public class DayforceConnector : IConnector
    {
        private readonly CatalogSourceSettings catalogSourceSettings;
        private readonly AppSettings appSettings;
        public bool Offline { get; set; } = true;

        public DayforceConnector(IOptions<CatalogSourceSettings> catalogSettings, IOptions<AppSettings> appSettings)
        {
            this.catalogSourceSettings = catalogSettings.Value;
            this.appSettings = appSettings.Value;
        }

        public Task<int> Load()
        {
            if (this.appSettings.DefaultCatalogConnector.Offline)
            {

            }
            else
            {

            }

            return null;
        }
    }
}
