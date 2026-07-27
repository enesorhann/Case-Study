using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using CaseStudy.Constants;
using CaseStudy.Interfaces;
using CaseStudy.Models;

namespace CaseStudy.Services.RabbitMQ
{
    public class SurveyPublisher : IEventPublisher
    {
        private readonly RabbitMqSettings _settings;

        public SurveyPublisher(IOptions<RabbitMqSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default)
            where TEvent : class
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.SurveyQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes<object>(eventMessage);

            // Tek kuyruk kullanıldığı için tüketicinin hangi olayı aldığını
            // ayırt edebilmesi adına olay tipi mesaj özelliğine yazılır.
            var properties = new BasicProperties
            {
                Type = typeof(TEvent).Name,
                ContentType = "application/json",
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: RabbitMqConstants.SurveyQueueName,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }
    }
}
