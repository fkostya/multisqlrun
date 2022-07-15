using appui.shared.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace appui
{
    public class OfflineFilePageReader : IPageReader
    {
        public async Task<HtmlDocument> GetPageAsync(string path)
        {
            var doc = new HtmlDocument();
            doc.Load(path);

            return await Task.FromResult(doc);
        }
    }
}
