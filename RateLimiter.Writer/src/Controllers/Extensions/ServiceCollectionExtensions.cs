namespace RateLimiter.Writer.Controllers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection collection)
    {
        collection.AddSingleton<RateLimitMapper>();
        return collection;
    }
}