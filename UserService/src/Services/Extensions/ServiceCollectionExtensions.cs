using UserService.Services.Configuration;

namespace UserService.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.Configure<CacheSettings>(configuration.GetSection(nameof(CacheSettings)));

        collection.AddSingleton<IUserRateLimitService, UserRateLimitService>();
        collection.AddSingleton<IUserService, UserService>();
        return collection;
    }
}