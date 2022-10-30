using appui.connectors;
using appui.connectors.Utils;
using appui.models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using Xunit;

namespace appui.tests.Connector
{
    public class MsSqlQueryConnectorTests
    {
        [Fact]
        public void CreatingNewInstance_MocksArguments_InstanceIsNotNull()
        {
            var sqlConnectionWrapperMock = new Mock<SqlConnectionWrapper>();

            var func = new Func<string, SqlConnectionWrapper>((con) => sqlConnectionWrapperMock.Object);

            var msSqlConnectionMock = new Mock<MsSqlConnection>();
            var loggerMock = new Mock<ILogger<MsSqlQueryConnector>>();

            var ms_connector = new MsSqlQueryConnector(func, msSqlConnectionMock.Object, loggerMock.Object);
            Assert.NotNull(ms_connector);
        }

        [Fact]
        public async void InvokingConnector_EmptyQuery_EmptyResult()
        {
            var func = new Func<string, SqlConnectionWrapper>((connectionString) => new SqlConnectionWrapper(connectionString));
            var msSqlConnectionMock = new Mock<MsSqlConnection>();

            var ms = new MsSqlQueryConnector(func, msSqlConnectionMock.Object, null);
            var result = await ms.Invoke(string.Empty);
            Assert.Empty(result);
        }


        [Fact]
        public async void InvokingConnector_EmptyConnection_EmptyResult()
        {
            var func = new Func<string, SqlConnectionWrapper>((connectionString) => new SqlConnectionWrapper(connectionString));
            var msSqlConnectionMock = new Mock<MsSqlConnection>();

            var ms = new MsSqlQueryConnector(func, msSqlConnectionMock.Object, null);
            var result = await ms.Invoke("query");
            Assert.Empty(result);
        }


        [Fact]
        public async void InvokingConnector_NonEmptyQuery_NonEmptyResult()
        {
            var msSqlConnectionMock = new Mock<MsSqlConnection>();
            var sqlCommandWrapperMock = new Mock<SqlCommandWrapper>();
            var sqlDataReaderMock = new Mock<SqlDataReaderWrapper>();
            var sqlConnectionWrapperMock = new Mock<SqlConnectionWrapper>();

            sqlCommandWrapperMock.Setup(f => f.ExecuteReaderAsync()).ReturnsAsync(sqlDataReaderMock.Object);

            sqlConnectionWrapperMock.Setup(f => f.CreateCommand()).Returns(sqlCommandWrapperMock.Object);
            sqlConnectionWrapperMock.Setup(f => f.OpenAsync());

            msSqlConnectionMock.Setup(_ => _.DbDatabase).Returns("database-test");
            msSqlConnectionMock.Setup(_ => _.DbServer).Returns("server-test");
            msSqlConnectionMock.Setup(f => f.IsValid()).Returns(true);

            sqlDataReaderMock.SetupSequence(_ => _.ReadAsync()).ReturnsAsync(true).ReturnsAsync(false);
            sqlDataReaderMock.SetupSequence(_ => _.HasRows).Returns(true).Returns(false);

            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            dataTable.Columns.Add(new DataColumn("CodeName", typeof(string)));
            sqlDataReaderMock.Setup(r => r.GetSchemaTable()).Returns(dataTable);

            sqlDataReaderMock.Setup(r => r.GetValue(It.Is<string>(f => f == "Id"))).Returns(1);
            sqlDataReaderMock.Setup(r => r.GetValue(It.Is<string>(f => f == "CodeName"))).Returns("CodeNameValue");

            var func = new Func<string, SqlConnectionWrapper>((connection) => sqlConnectionWrapperMock.Object);
            var ms = new MsSqlQueryConnector(func, msSqlConnectionMock.Object, null);
            var result = await ms.Invoke("query");

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal("database-test", result[0]["database"]);
            Assert.Equal("server-test", result[0]["server"]);
            Assert.Equal(1, result[0]["Id"]);
            Assert.Equal("CodeNameValue", result[0]["CodeName"]);
        }
    }
}