using Microsoft.Extensions.Options;
using StackExchange.Redis;
using UserService.Repositories.Configuration;

namespace UserService.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
        collection.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));

        collection.AddSingleton<IConnectionMultiplexer>(sp => {
            var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            return ConnectionMultiplexer.Connect(settings.ConnectionString);
        });

        collection.AddSingleton<IUserRateLimitRepository, UserRateLimitRepository>();
        collection.AddSingleton<IUserRepository, UserRepository>();

        return collection;
    }
}