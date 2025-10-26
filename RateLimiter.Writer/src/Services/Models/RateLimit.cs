namespace RateLimiter.Writer.Services.Models;

public record RateLimit(string Route, int RequestsPerMinute);