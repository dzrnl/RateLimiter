namespace RateLimiter.Reader.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.Configure<RateLimiterSettings>(configuration.GetSection(nameof(RateLimiterSettings)));

        collection.AddSingleton<IRateLimitService, RateLimitService>();
        collection.AddHostedService<RateLimitHostedService>();
        return collection;
    }
}