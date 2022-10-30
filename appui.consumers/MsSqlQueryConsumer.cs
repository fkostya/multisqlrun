using appui.connectors;
using appui.models;
using appui.models.Interfaces;
using appui.models.Payloads;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace appui.consumers
{
    public class MsSqlQueryConsumer
    {
        private readonly ILogger<MsSqlQueryConsumer>? logger;
        private readonly IServiceProvider? serviceProvider;

        public MsSqlQueryConsumer(IServiceProvider? serviceProvider, ILogger<MsSqlQueryConsumer>? logger)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public void Consume(MsSqlMessagePayload payload, CancellationToken cancellationToken)
        {
            //return await Task.Run(() =>
            //{
            //    try
            //    {
                    //var connection = new MsSqlConnection
                    //{
                    //    DbServer = payload?.Connection?.DbServer,
                    //    DbDatabase = payload?.Connection?.DbDatabase,
                    //    DbUserName = payload?.Connection?.DbUserName,
                    //    DbPassword = payload?.Connection?.DbPassword
                    //};
                    //var connector = new MsSqlQueryConnector(connection, serviceProvider?.GetService<ILogger<MsSqlQueryConnector>>());
                    //connector.Invoke(payload?.Query).GetAwaiter().GetResult();

            //        return await Task.FromResult(true);
            //    }
            //    catch (Exception ex)
            //    {
            //        this.logger?.LogError($"Exception: {ex}");
            //    }
            //}, cancellationToken);
        }
    }
}