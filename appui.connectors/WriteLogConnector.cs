using Microsoft.Extensions.Logging;

namespace appui.connectors
{
    public class WriteLogConnector
    {
        private readonly ILogger _logger;

        public WriteLogConnector(ILogger<WriteLogConnector> logger)
        {
            this._logger = logger;
        }

        public void Invoke(string message)
        {
            this._logger.Log(LogLevel.Information, $"LogConnector: {message}");
        }
    }
}