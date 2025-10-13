using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Services;

public interface IRateLimitService
{
    Task<RateLimit> CreateRateLimitAsync(CreateRateLimitDto dto, CancellationToken cancellationToken = default);

    Task<RateLimit> GetRateLimitByRouteAsync(string route, CancellationToken cancellationToken = default);

    Task<RateLimit> UpdateRateLimitAsync(UpdateRateLimitDto dto, CancellationToken cancellationToken = default);

    Task DeleteRateLimitAsync(string route, CancellationToken cancellationToken = default);
}