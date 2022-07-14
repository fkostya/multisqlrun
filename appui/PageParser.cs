using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui
{
    public interface IPageRow
    {
        public string id { get; set; }
        public string key { get; set; }
        public string database { get; set; }
        public string server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
    }
    public class PageRow : IPageRow
    {
        public string id { get; set; }
        public string key { get; set; }
        public string database { get; set; }
        public string server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
    }

    public interface IPageParser
    {
        Task<IList<IPageRow>> Parse();
    }

    public class PageParser : IPageParser
    {
        public IPageReader reader;
        public PageParser(IPageReader reader)
        {
            this.reader = reader;
        }

        public async Task<IList<IPageRow>> Parse() {
            var htmlDoc = await this.reader.GetPageAsync();

            var list = new List<IPageRow>();

            var content = htmlDoc.DocumentNode.SelectNodes("//div[@id='divContent']");

            if (content != null && content.Count != 0)
            {
                var versions = parseQAEnvironments(content[0]);

                var trs = content[0].SelectNodes("//table[@id='TestInfrastructure']//tbody//tr");

                if (trs == null || trs.Count == 0)
                    return list;

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
                            var row = new PageRow()
                            {
                                id = node.Id,
                                key = node.ChildNodes.Count > 0 ? node.ChildNodes[0].InnerHtml : "",
                                database = database,
                                server = server,
                                Version= versions[index]
                            };
                            
                            list.Add(row);
                        }
                        index++;
                    }
                }
            }

            return list;
        }

        public Task<string> Save(List<IPageRow> list, string filepath)
        {
            return Task.FromResult<string>("foo");
        }

        private IList<string> parseQAEnvironments(HtmlNode header)
        {
            if (header  == null || string.IsNullOrWhiteSpace(header.InnerHtml))
                return null;

            var nodes = header.SelectNodes("//a[@class='toggle-vis']");

            var list = new List<string>();
            foreach (var _ in nodes)
            {
                list.Add(_.InnerText);
            }
            return list;
        }
    }
}
