using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Repositories;

public interface IRateLimitRepository
{
    public Task<RateLimit> AddAsync(CreateRateLimitDto dto, CancellationToken cancellationToken = default);

    public Task<RateLimit?> FindByRouteAsync(string route, CancellationToken cancellationToken = default);

    public Task<RateLimit> UpdateAsync(UpdateRateLimitDto model, CancellationToken cancellationToken = default);

    public Task<bool> DeleteAsync(string route, CancellationToken cancellationToken = default);
}