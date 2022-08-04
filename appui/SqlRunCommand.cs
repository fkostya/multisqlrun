using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace appui
{
    public interface II
    {
        void open(IList<IConnectionStringInfo> dbs);

        void run(string sqlquery, Action<object> progress);

        void save(string filepath);
    }



    public interface IRunCommand
    {
        Task<List<Tuple<string, string>>> RunQuery(string query, string server, string db, string user, string pwd, int timeout);
    }

    internal class SqlRunCommand : IRunCommand
    {
        public void Run(string sql, IList<IConnectionStringInfo> dbs, Action<int> notify)
        {
            foreach (var db in dbs)
            {
                var i = 0;

                notify?.Invoke(i);
                i++;
            }
        }

        public async Task<Tuple<string, string>> RunQueryFromFile(string filePath, string server, string db, string user, string pwd, int timeout = 300)
        {
            await Task.Delay(1);
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                UserID = user,
                Password = pwd,
                ConnectTimeout = timeout
            };

            using (var connection = new SqlConnection(builder.ConnectionString))
            {

            }

            return null;
        }

        public async Task<List<Tuple<string, string>>> RunQuery(string query, string server, string db, string user, string pwd, int timeout = 300)
        {
            var output = new List<Tuple<string, string>>();
            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = server,
                    InitialCatalog = db,
                    UserID = user,
                    Password = pwd,
                    ConnectTimeout = timeout
                };

                try
                {
                    using (var connection = new SqlConnection(builder.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(query, connection) { CommandType = CommandType.Text })
                        {
                            await connection.OpenAsync();

                            using (var reader = await cmd.ExecuteReaderAsync())
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
                                reader.Close();
                            }
                        }
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                }
            }
            catch
            {
            }

            return output;
        }
    }
}
