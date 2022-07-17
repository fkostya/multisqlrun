using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly ConnectionSourceOption config;

        public OfflineFilePageReader(IOptions<ConnectionSourceOption> options)
        {
            config = options?.Value;
        }

        public async Task<HtmlDocument> GetPageAsync()
        {
            var doc = new HtmlDocument();
            doc.Load(config.FileConnectionSource);

            return await Task.FromResult(doc);
        }
    }
}
