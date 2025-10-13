namespace RateLimiter.Writer.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddSingleton<IRateLimitService, RateLimitService>();
        return collection;
    }
}