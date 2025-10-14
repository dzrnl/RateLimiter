using MongoDB.Driver;
using RateLimiter.Writer.Repositories.Entities;
using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Repositories;

public class RateLimitRepository(IMongoDatabase database, RateLimitMapper mapper) : IRateLimitRepository
{
    private const string CollectionName = "rateLimits";

    public async Task<RateLimit> AddAsync(CreateRateLimitDto dto, CancellationToken cancellationToken)
    {
        var entity = mapper.ToEntity(dto);

        var collection = GetRateLimitsCollection();
        await collection.InsertOneAsync(
            entity,
            options: null,
            cancellationToken);

        return mapper.ToModel(entity);
    }

    public async Task<RateLimit?> FindByRouteAsync(string route, CancellationToken cancellationToken)
    {
        var collection = GetRateLimitsCollection();

        var entity = await collection
            .Find(x => x.Route == route)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : mapper.ToModel(entity);
    }

    public async Task<RateLimit?> UpdateAsync(UpdateRateLimitDto model, CancellationToken cancellationToken)
    {
        var entity = mapper.ToEntity(model);

        var collection = GetRateLimitsCollection();

        var filter = Builders<RateLimitEntity>.Filter
            .Eq(rl => rl.Route, entity.Route);
        var update = Builders<RateLimitEntity>.Update
            .Set(x => x.RequestsPerMinute, entity.RequestsPerMinute);

        var updated = await collection.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<RateLimitEntity> { ReturnDocument = ReturnDocument.After },
            cancellationToken);

        return updated is null ? null : mapper.ToModel(updated);
    }

    public async Task<bool> DeleteAsync(string route, CancellationToken cancellationToken)
    {
        var collection = GetRateLimitsCollection();
        var result = await collection.DeleteOneAsync(rl => route == rl.Route, cancellationToken);

        return result.DeletedCount > 0;
    }

    private IMongoCollection<RateLimitEntity> GetRateLimitsCollection()
        => database.GetCollection<RateLimitEntity>(CollectionName);
}