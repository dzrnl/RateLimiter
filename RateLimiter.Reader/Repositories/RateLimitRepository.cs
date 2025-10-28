using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RateLimiter.Reader.Repositories.Entities;
using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Repositories;

public class RateLimitRepository : IRateLimitRepository
{
    private readonly int _batchSize;

    private readonly IMongoCollection<RateLimitEntity> _collection;
    private readonly RateLimitMapper _mapper;

    public RateLimitRepository(
        IMongoDatabase database,
        RateLimitMapper mapper,
        IOptions<DatabaseSettings> options)
    {
        _batchSize = options.Value.BatchSize;

        _collection = database.GetCollection<RateLimitEntity>(options.Value.CollectionName);
        _mapper = mapper;
    }

    public async IAsyncEnumerable<RateLimit> GetAllAsync()
    {
        var options = new FindOptions<RateLimitEntity>
        {
            BatchSize = _batchSize
        };

        using var cursor = await _collection.FindAsync(
            FilterDefinition<RateLimitEntity>.Empty,
            options);

        while (await cursor.MoveNextAsync())
        {
            foreach (var entity in cursor.Current)
            {
                yield return _mapper.ToModel(entity);
            }
        }
    }

    public async IAsyncEnumerable<RateLimit> WatchChangesAsync()
    {
        var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<RateLimitEntity>>()
            .Match(cs =>
                cs.OperationType == ChangeStreamOperationType.Insert ||
                cs.OperationType == ChangeStreamOperationType.Update ||
                cs.OperationType == ChangeStreamOperationType.Replace);

        var options = new ChangeStreamOptions
        {
            FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
        };

        using var cursor = await _collection.WatchAsync(pipeline, options);

        while (await cursor.MoveNextAsync())
        {
            foreach (var change in cursor.Current)
            {
                if (change.FullDocument != null)
                {
                    yield return _mapper.ToModel(change.FullDocument);
                }
            }
        }
    }
}