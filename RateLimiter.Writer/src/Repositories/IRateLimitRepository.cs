using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Repositories;

public interface IRateLimitRepository
{
    Task<RateLimit> AddAsync(CreateRateLimitDto dto, CancellationToken cancellationToken);

    Task<RateLimit?> FindByRouteAsync(string route, CancellationToken cancellationToken);

    Task<RateLimit?> UpdateAsync(UpdateRateLimitDto model, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(string route, CancellationToken cancellationToken);
}