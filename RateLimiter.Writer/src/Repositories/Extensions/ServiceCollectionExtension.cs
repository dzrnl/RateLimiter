using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RateLimiter.Writer.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));

        collection.AddSingleton<IMongoClient>(sp => {
            var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            return new MongoClient(settings.Uri);
        });

        collection.AddSingleton<IMongoDatabase>(sp => {
            var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.Database);
        });

        collection.AddSingleton<RateLimitMapper>();
        collection.AddSingleton<IRateLimitRepository, RateLimitRepository>();

        return collection;
    }
}