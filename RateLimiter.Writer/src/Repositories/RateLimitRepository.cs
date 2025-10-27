using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RateLimiter.Writer.Repositories.Entities;
using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Repositories;

public class RateLimitRepository : IRateLimitRepository
{
    private readonly IMongoCollection<RateLimitEntity> _collection;
    private readonly RateLimitMapper _mapper;

    public RateLimitRepository(
        IMongoDatabase database,
        RateLimitMapper mapper,
        IOptions<DatabaseSettings> options)
    {
        _collection = database.GetCollection<RateLimitEntity>(options.Value.CollectionName);
        _mapper = mapper;
    }

    public async Task<RateLimit> AddAsync(CreateRateLimitDto dto, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(dto);

        await _collection.InsertOneAsync(
            entity,
            options: null,
            cancellationToken);

        return _mapper.ToModel(entity);
    }

    public async Task<RateLimit?> FindByRouteAsync(string route, CancellationToken cancellationToken)
    {
        var entity = await _collection
            .Find(x => x.Route == route)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : _mapper.ToModel(entity);
    }

    public async Task<RateLimit?> UpdateAsync(UpdateRateLimitDto model, CancellationToken cancellationToken)
    {
        var entity = _mapper.ToEntity(model);

        var filter = Builders<RateLimitEntity>.Filter
            .Eq(rl => rl.Route, entity.Route);
        var update = Builders<RateLimitEntity>.Update
            .Set(x => x.RequestsPerMinute, entity.RequestsPerMinute);

        var updated = await _collection.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<RateLimitEntity> { ReturnDocument = ReturnDocument.After },
            cancellationToken);

        return updated is null ? null : _mapper.ToModel(updated);
    }

    public async Task<bool> DeleteAsync(string route, CancellationToken cancellationToken)
    {
        var result = await _collection.DeleteOneAsync(rl => route == rl.Route, cancellationToken);
        return result.DeletedCount > 0;
    }
}