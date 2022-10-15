using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace appui.shared
{
    /// <summary>
    /// The class loads connection strings from 'my' internal web site or pre-loaded html page.
    /// </summary>
    public class DFConnector : IConnector
    {
        private const int VALID_DATABASE_MIN_LENGTH_NAME = 3;
        private const int VALID_SERVER_MIN_LENGTH_NAME = 3;
        private readonly IPageReader reader;

        public DFConnector(IPageReader reader)
        {
            this.reader = reader;
        }

        public async Task<IList<IConnectionStringInfo>> LoadConnectionStrings(Dictionary<string, object> args)
        {
            var htmlDoc = await reader.LoadPageAsync();
            var list = new List<IConnectionStringInfo>();

            if (htmlDoc == null || htmlDoc.DocumentNode == null || !htmlDoc.DocumentNode.HasChildNodes)
            {
                return list;
            }

            var content = htmlDoc.DocumentNode.SelectNodes("//div[@id='divContent']")?[0];

            var trs = content?.SelectNodes("//table[@id='TestInfrastructure']//tbody//tr");

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
                        && !string.IsNullOrWhiteSpace(server) && server.Length > VALID_SERVER_MIN_LENGTH_NAME
                        && versions[index].Equals(args["version"].ToString(), StringComparison.OrdinalIgnoreCase))
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

            if (string.IsNullOrWhiteSpace(content?.InnerHtml))
                return list;

            var nodes = content?.SelectNodes("//a[@class='toggle-vis']");

            foreach (var _ in nodes)
            {
                list.Add(_.InnerText);
            }
            return list;
        }
    }
}