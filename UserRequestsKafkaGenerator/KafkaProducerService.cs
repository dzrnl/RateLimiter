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
        var userRequest = new UserRequest(0, "Service started");
        await _kafkaProducer.ProduceAsync(userRequest, cancellationToken);
        _logger.LogInformation("KafkaProducerService started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(0, "Service stopped");
        await _kafkaProducer.ProduceAsync(userRequest, cancellationToken);
        _kafkaProducer.Dispose();
    }
}