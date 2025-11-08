using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace UserRequestsKafkaGenerator;

public interface IKafkaProducer : IDisposable
{
    Task ProduceAsync(string message, CancellationToken cancellationToken);
}

public sealed class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducer(IOptions<KafkaSettings> options)
    {
        var settings = options.Value;
        _topic = settings.Topic;

        var config = new ProducerConfig
        {
            BootstrapServers = settings.BootstrapServers,
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public Task ProduceAsync(string message, CancellationToken cancellationToken)
    {
        return _producer.ProduceAsync(
            _topic, 
            new Message<Null, string> { Value = message }, 
            cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}