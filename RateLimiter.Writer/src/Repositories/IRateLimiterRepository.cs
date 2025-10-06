using RateLimiter.Writer.Services.Models;

namespace RateLimiter.Writer.Repositories;

public interface IRateLimiterRepository
{
    public Task<RateLimit?> GetAsync(string route, CancellationToken cancellationToken = default);

    public Task AddAsync(RateLimit model, CancellationToken cancellationToken = default);

    public Task<bool> UpdateAsync(RateLimit model, CancellationToken cancellationToken = default);

    public Task<bool> DeleteAsync(string route, CancellationToken cancellationToken = default);
}