using appui.connectors;
using appui.models.Interfaces;
using appui.models.Payloads;
using data_access_layer.Microsoft.SQL;
using data_access_layer.Microsoft.SQL.Wrappers;
using data_access_layer.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appui
{
    public class SingleThreadContext : IMessageProducer
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public SingleThreadContext(IServiceProvider serviceProvider, ILogger<SingleThreadContext> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task Publish(IMessagePayload payload)
        {
            try
            {
                var connection = (payload as MsSqlMessagePayload).Connection;

                MsSqlDataAccessLayer connector = new(
                                    new MsSqlConnectionWrapper(
                                        new MsSqlConnectionString(
                                            connection.DbDatabase,
                                            connection.DbServer,
                                            connection.DbDatabase,
                                            connection.DbUserName,
                                            connection.DbPassword,
                                            new Guid().ToString())));

                var output = await connector.RunSqlQueryAsDataSetAsync(payload.Query);

                var result = (List<Dictionary<string, object>>)output
                                        .Select(f =>
                                        {
                                            var list = new List<Tuple<string, string>>();
                                            foreach (var k in f.Rows)
                                            {
                                                foreach (var item in k)
                                                {
                                                    list.Add(new Tuple<string, string>(item.Key, item.Value.ToString()));
                                                }
                                            }
                                            return list;
                                        });

                var cvsConnector = new CvsFileWriteConnector(_serviceProvider.GetRequiredService<ILogger<CvsFileWriteConnector>>());
                await cvsConnector.Invoke(result, (payload as MsSqlMessagePayload).StoragePath);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error: {ex}");
            }

        }
    }
}