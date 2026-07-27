using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CaseStudy.Constants;
using CaseStudy.Models;

namespace CaseStudy.Services.RabbitMQ
{
    public class SurveyConsumer : BackgroundService
    {
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<SurveyConsumer> _logger;

        public SurveyConsumer(IOptions<RabbitMqSettings> settings, ILogger<SurveyConsumer> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = _settings.HostName,
                        Port = _settings.Port,
                        UserName = _settings.UserName,
                        Password = _settings.Password,
                        VirtualHost = _settings.VirtualHost
                    };

                    await using var connection = await factory.CreateConnectionAsync(stoppingToken);
                    await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                    await channel.ExchangeDeclareAsync(
                        exchange: RabbitMqConstants.ExchangeName,
                        type: RabbitMqConstants.ExchangeType,
                        durable: true,
                        autoDelete: false,
                        cancellationToken: stoppingToken);

                    await channel.QueueDeclareAsync(
                        queue: RabbitMqConstants.SurveyQueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        cancellationToken: stoppingToken);

                    await channel.QueueBindAsync(
                        queue: RabbitMqConstants.SurveyQueueName,
                        exchange: RabbitMqConstants.ExchangeName,
                        routingKey: RabbitMqConstants.SurveyBindingKey,
                        cancellationToken: stoppingToken);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += (_, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body.Span);
                        _logger.LogInformation("Received message: {Message}", message);
                        return Task.CompletedTask;
                    };

                    await channel.BasicConsumeAsync(
                        queue: RabbitMqConstants.SurveyQueueName,
                        autoAck: true,
                        consumer: consumer,
                        cancellationToken: stoppingToken);

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "RabbitMQ consumer could not connect. Retrying in {Delay}s.",
                        _settings.RetryDelaySeconds);

                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_settings.RetryDelaySeconds), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }
    }
}
