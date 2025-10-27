using System.Collections.Concurrent;
using RateLimiter.Reader.Repositories;
using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IRateLimitRepository _rateLimitRepository;
    private readonly ConcurrentDictionary<string, RateLimit> _cache = new();

    public RateLimitService(IRateLimitRepository repository)
    {
        _rateLimitRepository = repository;
    }
    
    public async Task InitializeAsync()
    {
        await foreach (var limit in _rateLimitRepository.GetAllAsync())
        {
            _cache[limit.Route] = limit;
        }
    }
    
    public IReadOnlyCollection<RateLimit> GetAllLimits()
    {
        return _cache.Values.ToList().AsReadOnly();
    }
}