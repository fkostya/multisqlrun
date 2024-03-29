﻿using appui.connectors.Utils;
using appui.models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;

namespace appui.connectors
{
    [Obsolete]
    public class MsSqlQueryConnector
    {
        const int timeout = 300;
        private readonly ILogger<MsSqlQueryConnector>? logger;
        private readonly MsSqlConnection connection;
        private readonly Func<string, SqlConnectionWrapper> factory;

        public MsSqlQueryConnector(Func<string, SqlConnectionWrapper> factory, MsSqlConnection connection, ILogger<MsSqlQueryConnector>? logger)
        {
            this.factory = factory;
            this.connection = connection;
            this.logger = logger;
        }

        public async Task<List<Dictionary<string, object>>> Invoke(string? query)
        {
            var result = new List<Dictionary<string, object>>();

            if (string.IsNullOrEmpty(query) || !connection.IsValid())
            {
                return result;
            }

            Stopwatch sw = new();
            sw.Start();

            try
            {
                using (var sqlconnection = factory.Invoke(this.connection.GetConnectionString<string>()))
                {
                    if (sqlconnection == null) throw new ArgumentNullException(nameof(sqlconnection));
                    using (var cmd = sqlconnection.CreateCommand())
                    {
                        if (cmd == null)
                        {
                            throw new ArgumentNullException("Could not create sql command");
                        }
                        cmd.CommandText = query;

                        await sqlconnection.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            DataTable schemaTable = reader.GetSchemaTable();

                            while (await reader.ReadAsync())
                            {
                                if (reader.HasRows)
                                {
                                    var dic = new Dictionary<string, object> {
                                        { "database", connection?.DbDatabase ?? string.Empty},
                                        { "server", connection?.DbServer ?? string.Empty }
                                    };

                                    foreach (DataColumn column in schemaTable.Columns)
                                    {
                                        dic[column.ColumnName.ToString()] = reader.GetValue(column.ColumnName.ToString());
                                    }
                                    result.Add(dic);
                                }
                                reader.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError($"Exception for connectionString:{connection?.GetConnectionString<string>()}, {ex.Message}");
            }
            finally
            {
                sw.Stop();
                logger?.LogTrace($"total query run time for {connection?.ConnectionName}", sw.Elapsed);
            }

            return result;
        }
    }
}