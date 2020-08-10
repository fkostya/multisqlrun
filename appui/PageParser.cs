using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appui
{
    public class PageRow
    {
        public string id { get; set; }

        public string client { get; set; }

        public string database { get; set; }

        public string server { get; set; }
    }

    public static class PageParser
    {
        public static Dictionary<string, List<PageRow>> Parse(HtmlDocument htmlDoc)
        {
            var parsed = new Dictionary<string, List<PageRow>>();

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='divContent']");

            if (nodes != null && nodes.Count != 0)
            {
                parseQAEnvironments(nodes[0], parsed);

                var _nodes = nodes[0].SelectNodes("//table[@id='TestInfrastructure']//tbody//tr");

                if (_nodes == null || _nodes.Count == 0)
                    return parsed;

                foreach (var node in _nodes)
                {
                    var id = node.Id;
                    var name = node.ChildNodes.Count > 0 ? node.ChildNodes[0].InnerHtml : "";

                    var memberNodes = node.SelectNodes(".//td[@data-site='sitename']");

                    var index = 0;
                    foreach (var mem in memberNodes)
                    {
                        var database = mem.SelectSingleNode(".//font[@data-client-id=\'" + id + "\']")?.InnerHtml;
                        var server = mem.SelectSingleNode(".//small[@class='dbServerVersion']")?.InnerHtml.Split("-")?[0]?.Trim().Replace("[", "");

                        if (!string.IsNullOrWhiteSpace(database) && database.Length > 3 && !string.IsNullOrWhiteSpace(server) && server.Length > 3)
                        {
                            var row = new PageRow()
                            {
                                id = id,
                                client = name,
                                database = database,
                                server = server
                            };

                            if(index < parsed.Keys.Count)
                                parsed[parsed.Keys.ElementAt(index)].Add(row);
                        }

                        index++;
                    }
                }
            }

            return parsed;
        }

        private static void parseQAEnvironments(HtmlNode root, Dictionary<string, List<PageRow>> parsed)
        {
            if (root == null || string.IsNullOrWhiteSpace(root.InnerHtml))
                return;

            var nodes = root.SelectNodes("//a[@class='toggle-vis']");

            foreach (var _ in nodes)
            {
                parsed[_.InnerText] = new List<PageRow>();
            }

        }
    }
}
