using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Services;

public interface IRateLimitService
{
    Task<RateLimit> CreateRateLimitAsync(CreateRateLimitDto dto, CancellationToken cancellationToken);

    Task<RateLimit> GetRateLimitByRouteAsync(string route, CancellationToken cancellationToken);

    Task<RateLimit> UpdateRateLimitAsync(UpdateRateLimitDto dto, CancellationToken cancellationToken);

    Task DeleteRateLimitAsync(string route, CancellationToken cancellationToken);
}