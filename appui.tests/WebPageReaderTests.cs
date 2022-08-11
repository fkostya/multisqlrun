using appui.shared;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace appui.tests
{
    [TestFixture]
    public class WebPageReaderTests
    {
        private WebPageReader buildWebPageReader(IOptions<List<CatalogConnection>>options = null, CredentialCache credentialCache = null, HtmlWeb htmlWeb = null)
        {
            var logger = new Mock<ILogger<AppErrorLog>>();

            return new WebPageReader(options, credentialCache, htmlWeb, logger.Object);
        }

        [TestCase]
        public void CreatingNewInstance_WhenNoArgumentsProvided_InstanceIsNotNull()
        {
            var logger = new Mock<ILogger<AppErrorLog>>();
            WebPageReader reader = buildWebPageReader();

            Assert.IsNotNull(reader);
        }

        [TestCase]
        public void CreatingNewInstance_WhenConnectionTypeNotProvided_InstanceIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>());

            //credential.Setup(c => c.GetCredential(new Uri("https://google.com"), "Basic"));
            //.Returns(new NetworkCredential());



            //var htmlWeb = new Mock<HtmlWeb>()
            //    .Setup(c => c.LoadFromWebAsync("", credential.Object))
            //    .Returns(new HtmlDocument());

            WebPageReader reader = buildWebPageReader(options);

            Assert.IsNotNull(reader);
        }

        [TestCase]
        public void CreatingNewInstance_WhenConnectionTypeNotSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-test" } });

            WebPageReader reader = buildWebPageReader(options);

            Assert.IsNotNull(reader);
        }

        [TestCase]
        public void CreatingNewInstance_WhenConnectionTypeSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-web" } });
            var credential = new Mock<CredentialCache>();
            var webHtml = new Mock<HtmlWeb>();

            WebPageReader reader = buildWebPageReader(options, credential.Object);

            Assert.IsNotNull(reader);
        }

        [TestCase]
        public async Task LoadPageAsync_WhenUrlIsEmpty_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-web", Url = "" } });
            var credential = new Mock<CredentialCache>();
            var webHtml = new Mock<HtmlWeb>();

            WebPageReader reader = buildWebPageReader(options, credential.Object, webHtml.Object);

            var doc = await reader.LoadPageAsync();
            Assert.IsNotNull(doc);
        }

        [TestCase]
        public async Task LoadPageAsync_WhenWebPageIsNull_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-web", Url = "" } });
            var credential = new Mock<CredentialCache>();
            var webHtml = new Mock<HtmlWeb>();

            WebPageReader reader = buildWebPageReader(options, credential.Object, null);

            var doc = await reader.LoadPageAsync();
            Assert.IsNotNull(doc);
        }
    }
}
