using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UserRequestsKafkaGenerator;

public class KafkaProducerService : IHostedService
{
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IKafkaProducer kafkaProducer, ILogger<KafkaProducerService> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _kafkaProducer.ProduceAsync("Service started", cancellationToken);
        _logger.LogInformation("KafkaProducerService started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _kafkaProducer.ProduceAsync("Service stopped", cancellationToken);
        _logger.LogInformation("KafkaProducerService stopped");
        _kafkaProducer.Dispose();
    }
}