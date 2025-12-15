using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Services;

public interface IRateLimitService
{
    Task LoadInitialCacheAsync();

    void StartWatchingForUpdates();

    Task ProcessUserRequestAsync(UserRequest request);

    IReadOnlyCollection<RateLimit> GetAllLimits();
}