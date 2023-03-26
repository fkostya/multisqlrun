using appui.shared.HostedEnvironment;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace appui.tests.HostedEnvironment
{
    public class EnvSetupFactoryTests
    {
        [Fact]
        public void When_CreatingNewInstanceLoggerIsNull_NewInstance()
        {
            var factory = new EnvSetupFactory(null, null);

            Assert.NotNull(factory);
        }

        [Fact]
        public void When_CreatingNewInstanceLoggerIsNotNull_NewInstance()
        {
            var loggerMock = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var factory = new EnvSetupFactory(loggerMock.Object, optionMock.Object);

            Assert.NotNull(factory);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("test", true)]
        public void When_RegisterHandlerInlineName_ReturnAsInlineData(string handlerName, bool expected)
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);
            var handlerMock = new Mock<IEnvSetupHandler>();

            var result = mock.RegisterHandler(handlerName, handlerMock.Object);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void When_RegisterSameHandlerNameAlreadyRegister_ReturnFalse()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);
            var handlerMock = new Mock<IEnvSetupHandler>();

            _ = mock.RegisterHandler("test", handlerMock.Object);
            var result = mock.RegisterHandler("test", handlerMock.Object);

            Assert.False(result);
        }


        [Fact]
        public void When_RegisterHandlerHandlerTypeAlreadyRegister_ReturnFalse()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);
            var handlerMock = new Mock<IEnvSetupHandler>();

            _ = mock.RegisterHandler("one", handlerMock.Object);
            var result = mock.RegisterHandler("two", handlerMock.Object);

            Assert.False(result);
        }

        [Fact]
        public void When_RegisterHandlerIsnull_ReturnFalse()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            var result = mock.RegisterHandler("test", null);

            Assert.False(result);
        }

        [Fact]
        public void When_RegisterHandlerReachMaxHandlers_ReturnFalse()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var handlerMock = new Mock<IEnvSetupHandler>();
            optionMock
                .Setup(option => option.Value)
                .Returns(new AppSettings
                {
                    MaxHandlers = 1
                });

            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            _ = mock.RegisterHandler("one", handlerMock.Object);
            var result = mock.RegisterHandler("two", handlerMock.Object);

            Assert.False(result);
        }

        [Fact]
        public void When_GetHandler_SingleHandler()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            var handlerMock = new Mock<IEnvSetupHandler>();
            mock.RegisterHandler("test", handlerMock.Object);
            Assert.Single(mock.GetHandlers());
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public void When_UnregisterHandlerDifferentName_ExpecteInlineData(string handlerName, bool expected)
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            var result = mock.UnregisterHandler(handlerName);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void When_RegisterHandlerNameIsTooooLong_ThrowException()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);
            var handlerMock = new Mock<IEnvSetupHandler>();

            var handlerName = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();

            Assert.Throws<ArgumentOutOfRangeException>(() => mock.RegisterHandler(handlerName, handlerMock.Object));
        }

        [Fact]
        public void When_RegisterHandlerReachMaxHandlers_WriteInLogger()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var handlerMock = new Mock<IEnvSetupHandler>();
            optionMock
                .Setup(option => option.Value)
                .Returns(new AppSettings
                {
                    MaxHandlers = 1
                });

            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            _ = mock.RegisterHandler("one", handlerMock.Object);
            var result = mock.RegisterHandler("two", handlerMock.Object);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            logger.Verify(
                    m => m.Log(
                            LogLevel.Information,
                            It.IsAny<EventId>(),
                            It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("reached MAX limit of handlers")),
                            null,
                            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                            Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }

        [Fact]
        public void When_RegisterHandlerHandlerAlreadyRegistered_WriteInLogger()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var handlerMock = new Mock<IEnvSetupHandler>();
            optionMock
                .Setup(option => option.Value)
                .Returns(new AppSettings
                {
                    MaxHandlers = 1
                });

            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            _ = mock.RegisterHandler("one", handlerMock.Object);
            var result = mock.RegisterHandler("two", handlerMock.Object);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            logger.Verify(
                    m => m.Log(
                            LogLevel.Information,
                            It.IsAny<EventId>(),
                            It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("reached MAX limit of handlers")),
                            null,
                            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                            Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }

        [Fact]
        public void When_RegisterHandlerWithSameNameAlreadyRegistered_WriteInLogger()
        {
            var logger = new Mock<ILogger<EnvSetupFactory>>();
            var optionMock = new Mock<IOptions<AppSettings>>();
            var handlerMock = new Mock<IEnvSetupHandler>();
            optionMock
                .Setup(option => option.Value)
                .Returns(new AppSettings
                {
                    MaxHandlers = 1
                });

            var mock = new EnvSetupFactory(logger.Object, optionMock.Object);

            _ = mock.RegisterHandler("one", handlerMock.Object);
            var result = mock.RegisterHandler("two", handlerMock.Object);

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            logger.Verify(
                    m => m.Log(
                            LogLevel.Information,
                            It.IsAny<EventId>(),
                            It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("reached MAX limit of handlers")),
                            null,
                            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                            Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }
    }
}
