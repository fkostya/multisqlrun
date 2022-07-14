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
        Task<HtmlDocument> GetPageAsync();
    }

    public class WebPageReader : IPageReader
    {
        private string _url = Config.Url;

        public async Task<HtmlDocument> GetPageAsync()
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(_url);

            return doc;
        }
    }

    public class OfflineFilePageReader : IPageReader
    {
        private string _offlineFilePath = Config.OfflineFilePath;

        public async Task<HtmlDocument> GetPageAsync()
        {
            var doc = new HtmlDocument();
            doc.Load(_offlineFilePath);

            return await Task.FromResult(doc);
        }
    }
}
