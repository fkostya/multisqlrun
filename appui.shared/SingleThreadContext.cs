using appui.connectors;
using appui.models.Interfaces;
using Microsoft.Extensions.Logging;

namespace appui.shared
{
    public class SingleThreadContext : IMessageProducer
    {
        private readonly ILogger _logger;
        private readonly MsSqlQueryConnector _msSqlQueryConnector;

        public SingleThreadContext(MsSqlQueryConnector msSqlQueryConnector, ILogger<SingleThreadContext> logger)
        {
            _logger = logger;
            _msSqlQueryConnector = msSqlQueryConnector;
        }

        public void Publish(IMessagePayload message)
        {
            var output = _msSqlQueryConnector.Run(message.ToString()).GetAwaiter().GetResult();
        }
    }
}
