using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Repositories;

public interface IRateLimitRepository
{
    IAsyncEnumerable<RateLimit> GetAllAsync();

    IAsyncEnumerable<RateLimitChange> WatchChangesAsync();
}