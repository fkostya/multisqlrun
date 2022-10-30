using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace appui.connectors.Utils
{
    [ExcludeFromCodeCoverage]
    public class SqlConnectionWrapper : IDisposable
    {
        private readonly SqlConnection connection;

        public SqlConnectionWrapper()
            : this(string.Empty)
        {

        }

        public SqlConnectionWrapper(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public virtual async Task OpenAsync()
        {
            await connection.OpenAsync();
        }

        public virtual SqlCommandWrapper CreateCommand()
        {
            return new SqlCommandWrapper(connection.CreateCommand());
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        //public string ConnectionString
        //{
        //    get
        //    {
        //        return connection.ConnectionString;
        //    }

        //}
    }
}