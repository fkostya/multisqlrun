using appui.shared;
using appui.shared.Models;
using HtmlAgilityPack;
using Moq;
using Xunit;

namespace appui.tests
{
    public class RunMsSqlQueryConnectorTests
    {
        [Fact]
        public void RunQuery_ConnectionAndQueryNotProvided_EmptyResult()
        {
            var connector = new RunMsSqlQueryConnector(null);
            var result = connector.Run(null, null);
            Assert.NotNull(result);
        }
    }
}
