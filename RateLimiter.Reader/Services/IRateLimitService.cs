using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Services;

public interface IRateLimitService
{
    Task InitializeAsync();

    IReadOnlyCollection<RateLimit> GetAllLimits();
}