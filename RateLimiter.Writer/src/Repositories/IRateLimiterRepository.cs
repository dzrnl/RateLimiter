using RateLimiter.Writer.Services.Entities;

namespace RateLimiter.Writer.Repositories;

public interface IRateLimiterRepository
{
    public Task<RateLimitModel?> GetAsync(string route, CancellationToken ct = default);
    public Task AddAsync(RateLimitModel model, CancellationToken ct = default);
    public Task<bool> UpdateAsync(RateLimitModel model, CancellationToken ct = default);
    public Task<bool> DeleteAsync(string route, CancellationToken ct = default);
}