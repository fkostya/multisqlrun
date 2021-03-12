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
    public interface IPageReader
    {
        Task<HtmlDocument> GetPageAsync(string path);
    }

    static class PageReader
    {
        public static async Task<HtmlDocument> GetPageAsync(IPageReader reader, string path)
        {
            return await reader.GetPageAsync(path);
        }
    }


    public class WebPageReader : IPageReader
    {
        public async Task<HtmlDocument> GetPageAsync(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            return doc;
        }
    }

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
