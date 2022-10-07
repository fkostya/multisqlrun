using appui.shared;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace appui.tests
{
    public class OfflineFilePageReaderTests
    {
        private OfflineFilePageReader buildOfflineFilePageReader(IOptions<List<ResourceCatalog>>? options)
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
            var options = Options.Create(new List<ResourceCatalog>());

            var reader = buildOfflineFilePageReader(options);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeNotSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "df-test" } });

            var reader = buildOfflineFilePageReader(options);

            Assert.NotNull(reader);
        }

        [Fact]
        public void CreatingNewInstance_WhenConnectionTypeSupported_InstanceIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "windows-file" } });

            var reader = buildOfflineFilePageReader(options);

            Assert.NotNull(reader);
        }

        [Fact]
        public async Task LoadPageAsync_WhenUrlIsEmpty_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "df-windows-file", FilePath= "" } });

            var reader = buildOfflineFilePageReader(options);

            var doc = await reader.LoadPageAsync();
            Assert.NotNull(doc);
        }

        [Fact]
        public async Task LoadPageAsync_WhenWebPageIsNull_DefaultDocumentIsNotNull()
        {
            var options = Options.Create(new List<ResourceCatalog>() { new ResourceCatalog { Type = "df-windows-file", FilePath = "" } });

            var reader = buildOfflineFilePageReader(options);

            var doc = await reader.LoadPageAsync();
            Assert.NotNull(doc);
        }
    }
}
