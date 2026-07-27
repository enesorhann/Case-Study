using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CaseStudy.Constants;
using CaseStudy.Interfaces;
using CaseStudy.Models;

namespace CaseStudy.Services.RabbitMQ
{
    public class SurveyConsumer : BackgroundService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly RabbitMqSettings _settings;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SurveyConsumer> _logger;

        public SurveyConsumer(
            IOptions<RabbitMqSettings> settings,
            IServiceScopeFactory scopeFactory,
            ILogger<SurveyConsumer> logger)
        {
            _settings = settings.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // RabbitMQ, API'den sonra ayağa kalkabilir o yüzden bağlanana kadar tekrar denenir.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ConsumeAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "RabbitMQ consumer could not connect. Retrying in {Delay}s.",
                        _settings.RetryDelaySeconds);

                    await Task.Delay(TimeSpan.FromSeconds(_settings.RetryDelaySeconds), stoppingToken);
                }
            }
        }

        private async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            await using var connection = await factory.CreateConnectionAsync(stoppingToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.SurveyQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.Span);
                var eventType = args.BasicProperties.Type;

                _logger.LogInformation("Received message ({EventType}): {Message}", eventType, message);

                try
                {
                    await DispatchAsync(eventType, message, stoppingToken);
                }
                catch (Exception ex)
                {
                    // Tüketicinin tek bir hatalı mesaj yüzünden durmaması için
                    // hata log'lanır ve dinlemeye devam edilir.
                    _logger.LogError(ex, "Failed to process message ({EventType}): {Message}", eventType, message);
                }
            };

            await channel.BasicConsumeAsync(
                queue: RabbitMqConstants.SurveyQueueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        // Mesajı olay tipine göre çözümleyip ilgili dış servis çağrısına iletir.
        private async Task DispatchAsync(string? eventType, string message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<ISurveyNotificationService>();

            switch (eventType)
            {
                case nameof(SurveyCreatedEvent):
                    var createdEvent = JsonSerializer.Deserialize<SurveyCreatedEvent>(message, SerializerOptions)!;
                    await notificationService.NotifySurveyCreatedAsync(createdEvent, cancellationToken);
                    break;

                case nameof(SurveyUpdatedEvent):
                    var updatedEvent = JsonSerializer.Deserialize<SurveyUpdatedEvent>(message, SerializerOptions)!;
                    await notificationService.NotifySurveyUpdatedAsync(updatedEvent, cancellationToken);
                    break;

                default:
                    _logger.LogWarning("Unknown event type '{EventType}', message skipped.", eventType);
                    break;
            }
        }
    }
}
