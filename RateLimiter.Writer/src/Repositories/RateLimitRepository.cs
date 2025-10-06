using MongoDB.Driver;
using RateLimiter.Writer.Repositories.Entities;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Repositories;

public class RateLimitRepository(IMongoDatabase database, RateLimitMapper mapper) : IRateLimiterRepository
{
    public async Task<RateLimit?> GetAsync(string route, CancellationToken cancellationToken)
    {
        var collection = _GetRateLimitsCollection();
        var entity = await collection
            .Find(rateLimit => route == rateLimit.Route)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : mapper.ToModel(entity);
    }

    public async Task AddAsync(RateLimit model, CancellationToken cancellationToken)
    {
        var entity = mapper.ToEntity(model);
        var collection = _GetRateLimitsCollection();
        await collection.InsertOneAsync(
            entity,
            options: null,
            cancellationToken);
    }

    public async Task<bool> UpdateAsync(RateLimit model, CancellationToken cancellationToken)
    {
        var entity = mapper.ToEntity(model);
        var collection = _GetRateLimitsCollection();
        var filter = Builders<RateLimitEntity>.Filter.Eq(rl => rl.Route, entity.Route);
        var update = Builders<RateLimitEntity>.Update
            .Set(x => x.RequestsPerMinute, entity.RequestsPerMinute);

        var result = await collection.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = false },
            cancellationToken);

        return result.MatchedCount == 1 && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string route, CancellationToken cancellationToken)
    {
        var collection = _GetRateLimitsCollection();
        var result = await collection.DeleteOneAsync(rl => route == rl.Route, cancellationToken);

        return result.DeletedCount > 0;
    }

    private IMongoCollection<RateLimitEntity> _GetRateLimitsCollection()
        => database.GetCollection<RateLimitEntity>("rate_limits");
}