using appui.connectors;
using appui.connectors.Utils;
using appui.models.Interfaces;
using appui.models.Payloads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace appui.shared
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
                var connector = new MsSqlQueryConnector(_serviceProvider.GetService<Func<string, SqlConnectionWrapper>>(), (payload as MsSqlMessagePayload).Connection, _serviceProvider.GetService<ILogger<MsSqlQueryConnector>>());
                var output = await connector.Invoke(payload.ToString());

                var result = (List<Dictionary<string, object>>)output
                                        .Select(f =>
                                        {
                                            var list = new List<Tuple<string, string>>();
                                            foreach (var k in f.Keys)
                                            {
                                                if (f[k] != null)
                                                {
                                                    list.Add(new Tuple<string, string>(k, f[k].ToString()));
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