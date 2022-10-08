using appui.models.Interfaces;
using appui.shared.Interfaces;
using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace appui.shared.RabbitMQ
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/rabbitmq-event-bus-development-test-environment
    /// </summary>
    public sealed class RabbitMqProducer : IMessageProducer, IDisposable
    {
        private readonly RabbitMqSettings rabbitMqSettings;
        private readonly ILogger<RabbitMqProducer> logger;
        private readonly ConnectionFactory connectionFactory;
        private readonly IConnection connection;

        public RabbitMqProducer(IOptions<RabbitMqSettings> rabbitmqSettings, ILogger<RabbitMqProducer> logger)
        {
            this.rabbitMqSettings = rabbitmqSettings?.Value;
            this.logger = logger;
            this.connectionFactory = new ConnectionFactory() { 
                Uri = new Uri(rabbitMqSettings?.Uri)
            };
            this.connection = connectionFactory.CreateConnection();
        }

        public void Dispose()
        {
            this.connection?.Dispose();
        }

        public async Task Publish(IMessagePayload payload)
        {
            try
            {
                //https://github.com/dotnet-architecture/eShopOnContainers
                using (var channel = connection.CreateModel())
                {
                    logger?.LogInformation($"declaring queue: {rabbitMqSettings?.Queue}");
                    channel.QueueDeclare(rabbitMqSettings?.Queue, false, false, false, null);
                    var json = JsonSerializer.Serialize(payload);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(rabbitMqSettings?.Exchange, rabbitMqSettings?.Queue, null, body);
                    logger?.LogInformation($"published to exchange: {rabbitMqSettings?.Exchange}, body-json: {json}");
                }
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, ex?.Message);
            }
        }
    }
}