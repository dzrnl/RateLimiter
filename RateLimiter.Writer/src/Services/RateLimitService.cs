using RateLimiter.Writer.Repositories;

namespace RateLimiter.Writer.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IRateLimitRepository _rateLimitRepository;

    public RateLimitService(IRateLimitRepository rateLimitRepository)
    {
        _rateLimitRepository = rateLimitRepository;
    }
}