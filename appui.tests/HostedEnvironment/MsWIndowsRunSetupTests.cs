using appui.shared.HostedEnvironment;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace appui.tests.HostedEnvironment
{
    public class MsWIndowsRunSetupTests
    {
        [Fact]
        public void RunSetup_CreateNewInstance_CreatedNewInstance()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var factoryMock = new Mock<IEnvSetupFactory>();
            var hostedMock = new Mock<IHostedEnvironment>();
            var mock = new MsWindowsRunSetup(serviceProviderMock.Object, factoryMock.Object, hostedMock.Object);

            Assert.NotNull(mock);
        }

        [Fact]
        public async Task RunSetup_ServiceColllectionAndFactoryNull_ReturnsTheSameReference()
        {
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var hostedMock = new Mock<IHostedEnvironment>();
            var mock = new MsWindowsRunSetup(serviceProviderMock.Object, null, hostedMock.Object);

            var result = await mock.RunSetup(serviceCollectionMock.Object);

            Assert.Same(serviceCollectionMock.Object, result);
        }

        [Fact]
        public async Task RunSetup_ServiceColllectionFactoryNull_NoException()
        {
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var hostedMock = new Mock<IHostedEnvironment>();
            var mock = new MsWindowsRunSetup(serviceProviderMock.Object, null, hostedMock.Object);

            var result = await mock.RunSetup(serviceCollectionMock.Object);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task RunSetup_FactoryProvided_ReturnsTheSameReference()
        {
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var factoryMock = new Mock<IEnvSetupFactory>();
            var hostedMock = new Mock<IHostedEnvironment>();
            var mock = new MsWindowsRunSetup(serviceProviderMock.Object, factoryMock.Object, hostedMock.Object);

            var result = await mock.RunSetup(serviceCollectionMock.Object);

            Assert.Same(serviceCollectionMock.Object, result);
        }

        [Fact]
        public void RunSetup_FactoryHandler_GetFactoryHandlerCount()
        {
            var stub = new ServiceCollection();
            var handlerMock = new Mock<IEnvSetupHandler>();
            var factoryMock = new Mock<IEnvSetupFactory>();

            factoryMock
                .Setup(x => x.GetHandlers())
                .Returns(new Dictionary<string, IEnvSetupHandler> { { "test", handlerMock.Object } });

            Assert.Equal(1, factoryMock.Object.GetHandlers()?.Count);
        }

        [Fact]
        public void RunSetup_FactoryHandler_HandlerExecutesOneTime()
        {
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var factoryMock = new Mock<IEnvSetupFactory>();
            var hostedMock = new Mock<IHostedEnvironment>();
            var mock = new MsWindowsRunSetup(serviceProviderMock.Object, factoryMock.Object, hostedMock.Object);

            var handlerMock = new Mock<IEnvSetupHandler>();
            factoryMock
                .Setup(f => f.GetHandlers())
                .Returns(new Dictionary<string, IEnvSetupHandler>(){
                            { "test", handlerMock.Object }
                        });

            _ = mock.RunSetup(serviceCollectionMock.Object);

            handlerMock.Verify(v => v.Execute(hostedMock.Object, serviceProviderMock.Object));
        }
    }
}