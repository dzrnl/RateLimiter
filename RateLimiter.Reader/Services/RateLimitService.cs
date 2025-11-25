using System.Collections.Concurrent;
using RateLimiter.Reader.Repositories;
using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly ConcurrentDictionary<string, RateLimit> _cache = new();
    private readonly ILogger<RateLimitService> _logger;

    public RateLimitService(
        IRateLimitRepository repository,
        ILogger<RateLimitService> logger)
    {
        _rateLimitRepository = repository;
        _logger = logger;
    }

    public async Task LoadInitialCacheAsync()
    {
        await foreach (var limit in _rateLimitRepository.GetAllAsync())
        {
            _cache[limit.Route] = limit;
        }
    }

    public void StartWatchingForUpdates()
    {
        Task.Factory.StartNew(
            async () =>
            {
                await foreach (var change in _rateLimitRepository.WatchChangesAsync())
                {
                    switch (change)
                    {
                        case UpsertRateLimit upsert:
                            _cache[upsert.Value.Route] = upsert.Value;
                            break;

                        case DeleteRateLimit del:
                            _cache.TryRemove(del.Route, out _);
                            break;
                    }
                }
            },
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
    }

    public Task ProcessUserRequestAsync(UserRequest request)
    {
        _logger.LogError("Processing user request: {Request}", request);
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<RateLimit> GetAllLimits()
        => _cache.Values.ToList().AsReadOnly();
}