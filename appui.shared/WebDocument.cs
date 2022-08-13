using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;

namespace appui.shared
{
    public class WebDocument : ILoadSource
    {
        private readonly HtmlNode content;
        private readonly HtmlNode defaultHtmlNode = HtmlNode.CreateNode("<html>");

        public WebDocument(HtmlDocument htmlDoc)
        {
            this.content = htmlDoc?.DocumentNode?.SelectNodes("//div[@id='divContent']")?[0];

            if(this.content == null)
            {
                this.content = defaultHtmlNode;
            }
        }

        private IList<string> GetVersions()
        {
            var nodes = this.content.SelectNodes("//a[@class='toggle-vis']");

            var list = new List<string>();
            if (nodes == null)
                return list;

            foreach (var _ in nodes)
            {
                list.Add(_.InnerText.Trim());
            }
            return list;
        }

        public IEnumerable<IConnectionStringInfo> GetConnections()
        {
            var trs = this.content.SelectNodes("//table[@id='TestInfrastructure']//tbody//tr");
            var list = new List<IConnectionStringInfo>();

            if (trs == null)
            {
                trs = new HtmlNodeCollection(defaultHtmlNode);
            }

            var versions = this.GetVersions();

            foreach (var node in trs)
            {
                var memberNodes = node.SelectNodes(".//td[@data-site='sitename']");
                var index = 0;

                if(memberNodes == null)
                {
                    memberNodes = new HtmlNodeCollection(this.defaultHtmlNode);
                }
                foreach (var mem in memberNodes)
                {
                    var database = mem.SelectSingleNode(".//font[@data-client-id=\'" + node.Id + "\']")?.InnerHtml.Trim();
                    var server = mem.SelectSingleNode(".//small[@class='dbServerVersion']")?.InnerHtml.Split("-")?[0]?.Trim().Replace("[", "");
                    var client = node.SelectSingleNode(".//td[@class='namespace sorting_1']")?.InnerHtml.Trim();

                    if (!string.IsNullOrWhiteSpace(database) && database.Length > 3 && !string.IsNullOrWhiteSpace(server) && server.Length > 3)
                    {
                        yield return new ConnectionStringInfo()
                        {
                            Id = node.Id.Trim(),
                            Client = client.Trim(),
                            Database = database.Trim(),
                            Server = server.Trim(),
                            Version = versions[index].Trim()
                        };
                    }
                    index++;
                }
            }
        }
    }
}
