using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace appui.shared
{
    /// <summary>
    /// The class loads connection strings from 'my' internal company web site or pre-loaded html page.
    /// </summary>
    public class DFConnector : IConnector
    {
        private readonly AppSettings appSettings;
        private readonly IServiceProvider serviceProvider;
        private const int VALID_DATABASE_MIN_LENGTH_NAME = 3;
        private const int VALID_SERVER_MIN_LENGTH_NAME = 3;

        public DFConnector(IOptions<AppSettings> appSettings, IServiceProvider serviceProvider)
        {
            this.appSettings = appSettings.Value;
            this.serviceProvider = serviceProvider;
        }

        public async Task<IList<IConnectionStringInfo>> LoadConnectionStrings()
        {
            IPageReader reader = appSettings.DefaultCatalogConnector.Offline ?
                serviceProvider.GetService<OfflineFilePageReader>() :
                serviceProvider.GetService<WebPageReader>();

            var htmlDoc = await reader.LoadPageAsync();

            var content = htmlDoc?.DocumentNode?.SelectNodes("//div[@id='divContent']")?[0];

            var trs = content.SelectNodes("//table[@id='TestInfrastructure']//tbody//tr");

            var list = new List<IConnectionStringInfo>();

            var versions = getVersions(content);

            foreach (var node in trs)
            {
                var memberNodes = node.SelectNodes(".//td[@data-site='sitename']");
                var index = 0;

                foreach (var mem in memberNodes)
                {
                    var database = mem.SelectSingleNode(".//font[@data-client-id=\'" + node.Id + "\']")?.InnerHtml;
                    var server = mem.SelectSingleNode(".//small[@class='dbServerVersion']")?.InnerHtml.Split("-")?[0]?.Trim().Replace("[", "");

                    if (!string.IsNullOrWhiteSpace(database) && database.Length > VALID_DATABASE_MIN_LENGTH_NAME 
                        && !string.IsNullOrWhiteSpace(server) && server.Length > VALID_SERVER_MIN_LENGTH_NAME)
                    {
                        list.Add(new ConnectionStringInfo()
                        {
                            Id = node.Id,
                            Client = node.ChildNodes.Count > 0 ? node.ChildNodes[0].InnerHtml : "",
                            Database = database,
                            Server = server,
                            Version = versions[index]
                        });
                    }
                    index++;
                }
            }
            return list;
        }

        private IList<string> getVersions(HtmlNode content)
        {
            var list = new List<string>();

            if (string.IsNullOrWhiteSpace(content.InnerHtml))
                return list;

            var nodes = content.SelectNodes("//a[@class='toggle-vis']");

            foreach (var _ in nodes)
            {
                list.Add(_.InnerText);
            }
            return list;
        }
    }
}
