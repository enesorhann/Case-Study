using System.Text.Json;
using RabbitMQ.Client;
using CaseStudy.Constants;
using CaseStudy.Interfaces;
using CaseStudy.Models;

namespace CaseStudy.Services.RabbitMQ
{
    public class SurveyPublisher : IEventPublisher
    {
        private readonly RabbitMqSettings _settings;

        public SurveyPublisher(Microsoft.Extensions.Options.IOptions<RabbitMqSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, string routingKey, CancellationToken cancellationToken = default)
            where TEvent : class
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: RabbitMqConstants.ExchangeName,
                type: RabbitMqConstants.ExchangeType,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes(@event);

            await channel.BasicPublishAsync(
                exchange: RabbitMqConstants.ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: new BasicProperties
                {
                    ContentType = "application/json"
                },
                body: body,
                cancellationToken: cancellationToken);
        }
    }
}
