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
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult?.Message?.Value is not { } jsonMessage)
                {
                    continue;
                }

                try
                {
                    var userRequest = JsonSerializer.Deserialize<UserRequest>(jsonMessage, JsonOptions);

                    if (userRequest is not null)
                    {
                        await _rateLimitService.ProcessUserRequestAsync(userRequest);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deserializing Kafka message");
                }
            }
            catch (OperationCanceledException)
            {
                break;
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