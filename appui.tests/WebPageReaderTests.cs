using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using Xunit;

namespace appui.tests
{
    public class WebPageReaderTests
    {
        private WebPageReader buildWebPageReader(IOptions<List<ResourceCatalog>>? options, CredentialCache? credentialCache, HtmlWeb? htmlWeb)
        {
            var logger = new Mock<ILogger<AppErrorLog>>();

            return new WebPageReader(options, credentialCache, htmlWeb, logger.Object);
        }

        [Fact]
        public void CreatingNewInstance_WhenNoArgumentsProvided_InstanceIsNotNull()
        {
            var logger = new Mock<ILogger<AppErrorLog>>();
            WebPageReader reader = buildWebPageReader(null, null, null);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeNotProvided_InstanceIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>());

            //credential.Setup(c => c.GetCredential(new Uri("https://google.com"), "Basic"));
            //.Returns(new NetworkCredential());



            //var htmlWeb = new Mock<HtmlWeb>()
            //    .Setup(c => c.LoadFromWebAsync("", credential.Object))
            //    .Returns(new HtmlDocument());

            WebPageReader reader = buildWebPageReader(options, null, null);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeNotSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "df-web-url-test" } });

            WebPageReader reader = buildWebPageReader(options, null, null);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "df-web-url" } });
            var credential = new Mock<CredentialCache>();
            var webHtml = new Mock<HtmlWeb>();

            WebPageReader reader = buildWebPageReader(options, credential.Object, null);

            Assert.NotNull(reader);
        }

        [Fact]
        public async Task LoadPageAsync_WhenUrlIsEmpty_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type= "df-web-url", Url = "" } });
            var credential = new Mock<CredentialCache>();
            var webHtml = new Mock<HtmlWeb>();

            WebPageReader reader = buildWebPageReader(options, credential.Object, webHtml.Object);

            var doc = await reader.LoadPageAsync();
            Assert.NotNull(doc);
        }

        [Fact]
        public async Task LoadPageAsync_WhenWebPageIsNull_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "df-web-url", Url = "" } });
            var credential = new Mock<CredentialCache>();
            var webHtml = new Mock<HtmlWeb>();

            WebPageReader reader = buildWebPageReader(options, credential.Object, null);

            var doc = await reader.LoadPageAsync();
            Assert.NotNull(doc);
        }
    }
}
