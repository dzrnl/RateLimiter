using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Services;

public interface IRateLimitService
{
    Task LoadInitialCacheAsync();

    void StartWatchingForUpdates();

    IReadOnlyCollection<RateLimit> GetAllLimits();
}