using appui.connectors;
using appui.models;
using Moq;
using Xunit;

namespace appui.tests
{
    public class RunMsSqlQueryConnectorTests
    {
        [Fact]
        public void RunQuery_ConnectionAndQueryNotProvided_EmptyResult()
        {
            var connectionMock = new Mock<MsSqlConnection>();
            var connector = new MsSqlQueryConnector(connectionMock.Object, null);
            var result = connector.Invoke(string.Empty);
            Assert.NotNull(result);
        }
    }
}
