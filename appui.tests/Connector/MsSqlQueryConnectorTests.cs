using appui.connectors;
using appui.models;
using Microsoft.Data.SqlClient;
using Moq;
using System.Data;
using Xunit;

namespace appui.tests.Connector
{
    public class MsSqlQueryConnectorTests
    {
        [Fact]
        public void CreatingNewInstance_WhenNoArgumentsProvided_InstanceIsNotNull()
        {
            var ms = new MsSqlQueryConnector(null, null);
            Assert.NotNull(ms);
        }

        [Fact]
        public async void Invoke_WhenNoArgumentsProvided_NotNullOutput()
        {
            var ms = new MsSqlQueryConnector(null, null);
            var result = await ms.Invoke(string.Empty);
            Assert.NotNull(result);
        }

        [Fact]
        public async void Invoke_WhenArgumentsProvided_NoException()
        {
            //https://stackoverflow.com/questions/58375054/mocking-sqlconnection-sqlcommand-and-sqlreader-in-c-sharp-using-mstest
            var mssqlConnectionMock = new Mock<MsSqlConnection>();
            var sqlConnectionMock = new Mock<SqlConnection>();
            var mock_connection_string = "Server=ServerName;Database=MSSQLDB;User Id=Username;Password=Password;";

            mssqlConnectionMock
                .Setup(f=>f.GetConnectionString<string>())
                .Returns(mock_connection_string);

            sqlConnectionMock
                .Setup(f => f.OpenAsync());

            var readerMock = new Mock<IDataReader>();
            readerMock.Setup(f => f.Read());

            var ms = new MsSqlQueryConnector(mssqlConnectionMock.Object, null);
            var result = await ms.Invoke(string.Empty);
            Assert.NotNull(result);
        }
    }
}