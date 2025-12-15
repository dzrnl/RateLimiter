using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using RateLimiter.Reader.Services;
using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Kafka;

public interface IRateLimitKafkaConsumer : IHostedService, IDisposable;

public sealed class RateLimitKafkaConsumer : BackgroundService, IRateLimitKafkaConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly string _topic;

    private readonly IRateLimitService _rateLimitService;
    private readonly ILogger<RateLimitKafkaConsumer> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public RateLimitKafkaConsumer(
        IRateLimitService rateLimitService,
        IOptions<KafkaSettings> kafkaSettings,
        ILogger<RateLimitKafkaConsumer> logger)
    {
        var settings = kafkaSettings.Value;
        _topic = settings.Topic;

        _rateLimitService = rateLimitService;
        _logger = logger;

        var config = new ConsumerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            GroupId = settings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Factory.StartNew(() => ConsumerLoop(stoppingToken), TaskCreationOptions.LongRunning);
    }

    private async Task ConsumerLoop(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(stoppingToken);

            if (consumeResult?.Message?.Value is not { } jsonMessage)
            {
                continue;
            }

            try
            {
                var userRequest = JsonSerializer.Deserialize<UserRequest>(jsonMessage, JsonOptions);

                if (userRequest is null)
                {
                    _logger.LogWarning("Empty message, skipping");
                    _consumer.Commit(consumeResult);
                    continue;
                }

                await _rateLimitService.ProcessUserRequestAsync(userRequest);
                _consumer.Commit(consumeResult);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Consume error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid message, skipping");
                _consumer.Commit(consumeResult);
            }
        }

        _consumer.Close();
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}