using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using RateLimiter.Reader.Repositories;
using RateLimiter.Reader.Repositories.Configuration;
using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Services;

public class RateLimitService : IRateLimitService
{
    private readonly TimeSpan _blockDuration;

    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly IRequestCounterRepository _requestCounterRepository;
    private readonly IUserBlockRepository _userBlockRepository;
    private readonly ConcurrentDictionary<string, RateLimit> _cache = new();
    private readonly ILogger<RateLimitService> _logger;

    public RateLimitService(
        IRateLimitRepository repository,
        IRequestCounterRepository requestCounterRepository,
        IUserBlockRepository userBlockRepository,
        IOptions<RateLimiterSettings> rateLimiterSettings,
        ILogger<RateLimitService> logger)
    {
        _blockDuration = rateLimiterSettings.Value.BlockDuration;
        _rateLimitRepository = repository;
        _requestCounterRepository = requestCounterRepository;
        _userBlockRepository = userBlockRepository;
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
            async () => {
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

    public async Task ProcessUserRequestAsync(UserRequest request)
    {
        _logger.LogInformation("Processing user request: {Request}", request);

        if (!_cache.TryGetValue(request.Endpoint, out var limit))
        {
            return;
        }

        var allowed = await _requestCounterRepository.TryConsumeRequestAsync(
            request.UserId,
            request.Endpoint,
            limit.RequestsPerMinute);

        if (!allowed)
        {
            await _userBlockRepository.BlockUserAsync(request.UserId, request.Endpoint, _blockDuration);
        }
    }

    public IReadOnlyCollection<RateLimit> GetAllLimits()
        => _cache.Values.ToList().AsReadOnly();
}