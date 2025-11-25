namespace RateLimiter.Reader.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaConsumer(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));

        collection.AddSingleton<IRateLimitKafkaConsumer, RateLimitKafkaConsumer>();
        collection.AddHostedService<RateLimitKafkaConsumer>();

        return collection;
    }
}