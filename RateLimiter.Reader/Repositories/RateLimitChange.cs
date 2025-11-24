using RateLimiter.Reader.Services.Models;

namespace RateLimiter.Reader.Repositories;

public abstract record RateLimitChange;

public sealed record UpsertRateLimit(RateLimit Value) : RateLimitChange;

public sealed record DeleteRateLimit(string Route) : RateLimitChange;