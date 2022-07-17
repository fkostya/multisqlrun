using appui.shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appui
{
    public interface IRunCommand
    {
        Task<List<Tuple<string, string>>> RunQuery(string sqlquery);
    }

    internal class SqlRunCommand : IRunCommand, IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlConnectionStringBuilder _connectionBuilder;
        public string Database
        {
            get { return _connection.Database; }
        }

        public SqlRunCommand(SqlConnectionStringBuilder connection)
        {
            _connection = new SqlConnection(connection.ConnectionString);
            _connectionBuilder = connection;
        }

        public void Dispose()
        {
            if(_connection != null)
            {
                _connection.Dispose();
            }
        }

        public async Task<List<Tuple<string, string>>> RunQuery(string sqlquery)
        {
            var output = new List<Tuple<string, string>>();
            try
            {
                var reader = await internalRun(sqlquery);

                try
                {
                    DataTable schemaTable = reader.GetSchemaTable();

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            foreach (DataRow row in schemaTable.Rows)
                            {
                                foreach (DataColumn column in schemaTable.Columns)
                                {
                                    var colname = row[column].ToString();

                                    output.Add(new Tuple<string, string>(colname, reader[colname].ToString()));
                                }
                            }
                        }
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            catch
            {
            }

            return output;
        }

        private async Task<SqlDataReader> internalRun(string query)
        {
            await _connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(query, this._connection))
            {
                cmd.CommandTimeout = _connectionBuilder.ConnectTimeout;

                await _connection.CloseAsync();
                
                return await cmd.ExecuteReaderAsync();
            }
        }
    }
}
