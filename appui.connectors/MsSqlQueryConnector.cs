using appui.models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;

namespace appui.connectors
{
    public class MsSqlQueryConnector
    {
        const int timeout = 300;
        private readonly ILogger<MsSqlQueryConnector>? logger;
        private readonly MsSqlConnection connection;

        public MsSqlQueryConnector(MsSqlConnection connection, ILogger<MsSqlQueryConnector>? logger)
        {
            this.logger = logger;
            this.connection = connection;
        }

        public async Task<List<Dictionary<string, object?>>> Run(string? query)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = new List<Dictionary<string, object?>>();
            try
            {
                using (var sqlconnection = new SqlConnection(connection.GetConnectionString<string>()))
                {
                    using (var cmd = new SqlCommand(query, sqlconnection) { CommandType = CommandType.Text })
                    {
                        await sqlconnection.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            DataTable schemaTable = reader.GetSchemaTable();

                            var dic = new Dictionary<string, object?>();
                            foreach (DataColumn column in schemaTable.Columns)
                            {
                                dic[column.ColumnName.ToString()] = null;
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
                logger?.LogError($"Exception for connectionString:{connection.GetConnectionString<string>()}, {ex.Message}");
            }
            finally
            {
                sw.Stop();
                logger?.LogTrace($"query run time for {connection.Name}", sw.Elapsed);
            }

            return result;
        }
    }
}