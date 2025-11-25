namespace RateLimiter.Reader.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));

        return collection;
    }
}