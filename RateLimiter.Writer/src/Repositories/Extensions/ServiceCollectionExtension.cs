using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using RateLimiter.Writer.Repositories.Entities;

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
            var db = sp.GetRequiredService<IMongoClient>().GetDatabase(settings.Database);

            var rateLimits = db.GetCollection<RateLimitEntity>("rateLimits");
            var indexKeys = Builders<RateLimitEntity>.IndexKeys.Ascending(x => x.Route);
            var indexModel = new CreateIndexModel<RateLimitEntity>(indexKeys, new CreateIndexOptions { Unique = true });
            rateLimits.Indexes.CreateOne(indexModel);

            return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.Database);
        });

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, _ => true);

        collection.AddSingleton<RateLimitMapper>();
        collection.AddSingleton<IRateLimitRepository, RateLimitRepository>();

        return collection;
    }
}