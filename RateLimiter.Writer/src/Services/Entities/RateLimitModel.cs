namespace RateLimiter.Writer.Services.Entities;

public record RateLimitModel(string Route, int RequestsPerMinute);