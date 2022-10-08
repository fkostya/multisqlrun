using appui.connectors;
using appui.models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace appui.consumers
{
    public class WriteLogConsumer
    {
        private readonly IServiceProvider? serviceProvider;
        private readonly ILogger<WriteLogConsumer>? logger;

        public WriteLogConsumer(IServiceProvider? serviceProvider, ILogger<WriteLogConsumer>? logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public void Consume(string message)
        {
            try
            {
                //var connector = this.serviceProvider?.GetService<CvsFileWriteConnector>();

                //connector.Invoke(message);
            }
            catch (Exception ex)
            {
                this.logger?.LogError($"WriteLogConsumer: {ex}");
            }
        }
    }
}
