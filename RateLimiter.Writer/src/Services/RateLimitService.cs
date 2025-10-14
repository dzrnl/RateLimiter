using RateLimiter.Writer.Repositories;
using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Services;

public class RateLimitService : IRateLimitService
{
    private readonly IRateLimitRepository _rateLimitRepository;

    public RateLimitService(IRateLimitRepository rateLimitRepository)
    {
        _rateLimitRepository = rateLimitRepository;
    }

    public Task<RateLimit> CreateRateLimitAsync(CreateRateLimitDto dto, CancellationToken cancellationToken)
    {
        // TODO: add check
        return _rateLimitRepository.AddAsync(dto, cancellationToken);
    }

    public async Task<RateLimit> GetRateLimitByRouteAsync(string route, CancellationToken cancellationToken = default)
    {
        var rateLimit = await _rateLimitRepository.FindByRouteAsync(route, cancellationToken);

        if (rateLimit is null)
        {
            throw new KeyNotFoundException(); // TODO: Exceptions
        }

        return rateLimit;
    }

    public Task<RateLimit> UpdateRateLimitAsync(UpdateRateLimitDto dto, CancellationToken cancellationToken = default)
    {
        return _rateLimitRepository.UpdateAsync(dto, cancellationToken);
    }

    public async Task DeleteRateLimitAsync(string route, CancellationToken cancellationToken = default)
    {
        var deleted = await _rateLimitRepository.DeleteAsync(route, cancellationToken);

        if (!deleted)
        {
            throw new KeyNotFoundException(); // TODO: Exceptions
        }
    }
}