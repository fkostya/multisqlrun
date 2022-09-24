using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace appui.shared
{
    public class RunMsSqlQueryConnector
    {
        const int timeout = 300;
        private readonly ILogger Logger;

        public RunMsSqlQueryConnector(ILogger<AppErrorLog> logger = null)
        {
            this.Logger = logger;
        }

        public async Task<List<Dictionary<string, object>>> Run(ITenantConnection connection, string query)
        {
            return await runQuery(connection, query);
        }

        private async Task<List<Dictionary<string, object>>> runQuery(ITenantConnection connection, string query)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = new List<Dictionary<string, object>>();
            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = connection.DbServer,
                    InitialCatalog = connection.Database,
                    UserID = connection.UserName,
                    Password = connection.Password,
                    ConnectTimeout = timeout
                };

                using (var sqlconnection = new SqlConnection(builder.ConnectionString))
                {
                    using (var cmd = new SqlCommand(query, sqlconnection) { CommandType = CommandType.Text })
                    {
                        await sqlconnection.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            DataTable schemaTable = reader.GetSchemaTable();

                            var dic = new Dictionary<string, object>();
                            foreach (DataColumn column in schemaTable.Columns)
                            {
                                if (!dic.ContainsKey(column.ColumnName.ToString()))
                                {
                                    dic.Add(column.ColumnName.ToString(), null);
                                }
                            }

                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                {
                                    foreach (var key in dic.Keys)
                                    {
                                        dic[key] = reader.GetValue(key);
                                    }

                                    result.Add(dic);
                                }
                            }
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.Message);
            }
            finally
            {
                sw.Stop();
                Logger?.LogTrace($"query run time for {connection.DbServer}", sw.Elapsed);
            }

            return result;
        }
    }
}