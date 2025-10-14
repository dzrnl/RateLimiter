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

    public async Task<RateLimit> GetRateLimitByRouteAsync(string route, CancellationToken cancellationToken)
    {
        var rateLimit = await _rateLimitRepository.FindByRouteAsync(route, cancellationToken);

        if (rateLimit is null)
        {
            throw new RateLimitNotFoundException(route);
        }

        return rateLimit;
    }

    public async Task<RateLimit> UpdateRateLimitAsync(UpdateRateLimitDto dto, CancellationToken cancellationToken)
    {
        var updatedLimit = await _rateLimitRepository.UpdateAsync(dto, cancellationToken);
        
        if (updatedLimit is null)
        {
            throw new RateLimitNotFoundException(dto.Route);
        }

        return updatedLimit;
    }

    public async Task DeleteRateLimitAsync(string route, CancellationToken cancellationToken)
    {
        var deleted = await _rateLimitRepository.DeleteAsync(route, cancellationToken);

        if (!deleted)
        {
            throw new RateLimitNotFoundException(route);
        }
    }
}