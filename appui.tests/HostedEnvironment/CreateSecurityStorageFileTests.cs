using appui.shared.HostedEnvironment;
using appui.shared.Models;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace appui.tests.HostedEnvironment
{
    public class CreateSecurityStorageFileTests
    {
        [Fact]
        public void When_CreatingNewInstance_NewInstanceCreated()
        {
            var optionMock = new Mock<IOptions<AppSettings>>();
            var logger = new Mock<ILogger<CreateSecurityStorageFile>>();

            var mock = new CreateSecurityStorageFile(optionMock.Object, logger.Object);

            Assert.NotNull(mock);
        }

        [Fact]
        public void When_CreatingNewInstanceNullArguments_NewInstanceCreated()
        {
            var mock = new CreateSecurityStorageFile(null, null);

            Assert.NotNull(mock);
        }

        [Fact]
        public void When_CreatingNewInstanceNullArguments_DefaultFileName()
        {
            var mock = new CreateSecurityStorageFile(null, null);

            Assert.Equal("config.json", mock.GetConfigFileName());
        }

        [Fact]
        public void When_CreatingNewInstanceWithFileName_FileNameFromCtor()
        {
            var optionMock = new Mock<IOptions<AppSettings>>();
            var logger = new Mock<ILogger<CreateSecurityStorageFile>>();

            optionMock
                .Setup(o => o.Value)
                .Returns(new AppSettings()
                {
                    JsonConfigFileName = "test.json"
                });

            var mock = new CreateSecurityStorageFile(optionMock.Object, logger.Object);

            Assert.Equal("test.json", mock.GetConfigFileName());
        }

        [Fact]
        public async Task When_ExecuteHostArgumentIsNull_NoException()
        {
            var optionMock = new Mock<IOptions<AppSettings>>();
            var logger = new Mock<ILogger<CreateSecurityStorageFile>>();
            var mock = new CreateSecurityStorageFile(optionMock.Object, logger.Object);

            await mock.Execute(null);
        }

        [Fact]
        public async Task When_ExecuteHostArgumentIsNotNull_ExecuteOneTimeCalled()
        {
            var optionMock = new Mock<IOptions<AppSettings>>();
            var logger = new Mock<ILogger<CreateSecurityStorageFile>>();
            var mock = new CreateSecurityStorageFile(optionMock.Object, logger.Object);

            var hosted = new Mock<IHostedEnvironment>();

            await mock.Execute(hosted.Object);

            hosted.Verify((v) => v.Execute(It.IsAny<object>()));
        }

        [Fact]
        public async Task When_HostExecuteThrowException_RecordInLogger()
        {
            var optionMock = new Mock<IOptions<AppSettings>>();
            var logger = new Mock<ILogger<CreateSecurityStorageFile>>();
            var mock = new CreateSecurityStorageFile(optionMock.Object, logger.Object);

            var hosted = new Mock<IHostedEnvironment>();
            hosted
                .Setup(h => h.Execute(It.IsAny<object>()))
                .Throws<Exception>();

            await mock.Execute(hosted.Object);

            //https://stackoverflow.com/questions/52707702/how-do-you-mock-ilogger-loginformation/58413842#58413842
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            logger.Verify(x => x.Log(
                   It.IsAny<LogLevel>(),
                   It.IsAny<EventId>(),
                   It.IsAny<It.IsAnyType>(),
                   It.IsAny<Exception>(),
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }
    }
}