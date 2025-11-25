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
        
        _logger.LogWarning("init Kafka consumer 0");


        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        
        _logger.LogWarning("init Kafka consumer");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogWarning("Creating Kafka consumer for topic {Topic}", _topic);
        _consumer.Subscribe(_topic);
        _logger.LogWarning("Started Kafka consumer");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                if (consumeResult?.Message?.Value is not { } jsonMessage)
                {
                    continue;
                }

                try
                {
                    _logger.LogWarning("Received Kafka Message: {Message}", jsonMessage);
                    var userRequest = JsonSerializer.Deserialize<UserRequest>(jsonMessage, JsonOptions);

                    if (userRequest is not null)
                    {
                        _logger.LogWarning("is not null");

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