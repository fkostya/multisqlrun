using appui.shared.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui.shared
{
    internal class WebDocument
    {
        private readonly HtmlNode content;

        public WebDocument(HtmlDocument htmlDoc)
        {
            this.content = htmlDoc?.DocumentNode?.SelectNodes("//div[@id='divContent']")?[0];

            if(this.content == null)
            {
                this.content = HtmlNode.CreateNode("");
            }
        }

        private IList<string> GetVersions()
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

        public IEnumerable<IPageRow> GetSites()
        {
            var trs = content.SelectNodes("//table[@id='TestInfrastructure']//tbody//tr");

            var list = new List<IPageRow>();

            var versions = this.GetVersions();

            foreach (var node in trs)
            {
                var memberNodes = node.SelectNodes(".//td[@data-site='sitename']");
                var index = 0;

                foreach (var mem in memberNodes)
                {
                    var database = mem.SelectSingleNode(".//font[@data-client-id=\'" + node.Id + "\']")?.InnerHtml;
                    var server = mem.SelectSingleNode(".//small[@class='dbServerVersion']")?.InnerHtml.Split("-")?[0]?.Trim().Replace("[", "");

                    if (!string.IsNullOrWhiteSpace(database) && database.Length > 3 && !string.IsNullOrWhiteSpace(server) && server.Length > 3)
                    {
                        yield return new PageRow()
                        {
                            id = node.Id,
                            key = node.ChildNodes.Count > 0 ? node.ChildNodes[0].InnerHtml : "",
                            database = database,
                            server = server,
                            Version = versions[index]
                        };
                    }
                    index++;
                }
            }
        }
    }
}
