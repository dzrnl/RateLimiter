using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using RateLimiter.Reader.Repositories.Configuration;
using StackExchange.Redis;

namespace RateLimiter.Reader.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
        collection.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));

        collection.AddSingleton<IMongoClient>(sp => {
            var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            return new MongoClient(settings.Uri);
        });

        collection.AddSingleton<IMongoDatabase>(sp => {
            var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.Database);
        });

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, _ => true);

        collection.AddSingleton<IConnectionMultiplexer>(sp => {
            var settings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            return ConnectionMultiplexer.Connect(settings.ConnectionString);
        });

        collection.AddSingleton<RateLimitsStatisticsRedisClient>();

        collection.AddSingleton<RateLimitMapper>();
        collection.AddSingleton<IRateLimitRepository, RateLimitRepository>();
        collection.AddSingleton<IRequestCounterRepository, RequestCounterRepository>();
        collection.AddSingleton<IUserBlockRepository, UserBlockRepository>();

        return collection;
    }
}