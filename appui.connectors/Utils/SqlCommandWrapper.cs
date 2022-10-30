using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace appui.connectors.Utils
{
    [ExcludeFromCodeCoverage]
    public class SqlCommandWrapper : IDisposable
    {
        public SqlCommand Command { get; }

        public SqlCommandWrapper()
            : this(new SqlCommand())
        {

        }
        public SqlCommandWrapper(SqlCommand command)
        {
            Command = command;
        }

        public string CommandText
        {
            get
            {
                return Command.CommandText;
            }
            set
            {
                Command.CommandText = value;
            }
        }

        public virtual async Task<SqlDataReaderWrapper> ExecuteReaderAsync()
        {
            return new SqlDataReaderWrapper(await Command.ExecuteReaderAsync());
        }

        public void Dispose()
        {
            this.Command.Dispose();
        }
    }
}