using MongoDB.Driver;
using RateLimiter.Writer.Repositories.DbModels;
using RateLimiter.Writer.Services.Entities;

namespace RateLimiter.Writer.Repositories;

public class RateLimitRepository(IMongoDatabase database, RateLimitMapper mapper) : IRateLimiterRepository
{
    public async Task<RateLimitModel?> GetAsync(string route, CancellationToken ct = default)
    {
        var collection = _GetRateLimitsCollection();
        var entity = await collection
            .Find(rateLimit => route == rateLimit.Route)
            .FirstOrDefaultAsync(cancellationToken: ct);
        
        return entity is null ? null : mapper.ToModel(entity);
    }
    
    public async Task AddAsync(RateLimitModel model, CancellationToken ct = default)
    {
        var entity = mapper.ToEntity(model);
        var collection = _GetRateLimitsCollection();
        await collection.InsertOneAsync(
            entity, 
            options: null,
            ct);
    }

    public async Task<bool> UpdateAsync(RateLimitModel model, CancellationToken ct = default)
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
            ct);

        return result.MatchedCount == 1 && result.ModifiedCount > 0;
    }
    
    public async Task<bool> DeleteAsync(string route, CancellationToken ct = default)
    {
        var collection = _GetRateLimitsCollection();
        var result = await collection.DeleteOneAsync(rl => route == rl.Route, ct);
        
        return result.DeletedCount > 0;
    }
    
    private IMongoCollection<RateLimitEntity> _GetRateLimitsCollection() 
        => database.GetCollection<RateLimitEntity>("rate_limits");
}
