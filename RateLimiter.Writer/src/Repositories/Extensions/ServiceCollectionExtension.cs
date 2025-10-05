using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RateLimiter.Writer.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        // Settings for MongoDB connection
        collection.AddOptions<MongoSettings>()
            .Bind(configuration.GetSection("MongoSettings"));
        
        // Mongo client
        collection.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoClient(settings.Uri);
        });

        // Mongo database
        collection.AddSingleton<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(settings.Database);
        });

        collection.AddSingleton<RateLimitMapper>();
        collection.AddSingleton<IRateLimiterRepository, RateLimitRepository>();
        
        return collection;
    }
}