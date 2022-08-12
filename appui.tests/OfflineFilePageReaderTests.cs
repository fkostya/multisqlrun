using appui.shared;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace appui.tests
{
    public class OfflineFilePageReaderTests
    {
        private OfflineFilePageReader buildOfflineFilePageReader(IOptions<List<CatalogConnection>>? options)
        {
            var logger = new Mock<ILogger<AppErrorLog>>();

            return new OfflineFilePageReader(null, logger.Object);
        }

        [Fact]
        public void CreatingNewInstance_WhenNoArgumentsProvided_InstanceIsNotNull()
        {
            OfflineFilePageReader reader = buildOfflineFilePageReader(null);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeNotProvided_InstanceIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>());

            var reader = buildOfflineFilePageReader(options);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeNotSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-test" } });

            var reader = buildOfflineFilePageReader(options);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-offline" } });

            var reader = buildOfflineFilePageReader(options);

            Assert.NotNull(reader);
        }

        [Fact]
        public async Task LoadPageAsync_WhenUrlIsEmpty_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-offline", FilePath= "" } });

            var reader = buildOfflineFilePageReader(options);

            var doc = await reader.LoadPageAsync();
            Assert.NotNull(doc);
        }

        [Fact]
        public async Task LoadPageAsync_WhenWebPageIsNull_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<CatalogConnection>() { new CatalogConnection { Name = "df-offline", FilePath = "" } });

            var reader = buildOfflineFilePageReader(options);

            var doc = await reader.LoadPageAsync();
            Assert.NotNull(doc);
        }
    }
}
