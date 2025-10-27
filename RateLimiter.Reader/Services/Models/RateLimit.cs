namespace RateLimiter.Reader.Services.Models;

public record RateLimit(string Route, int RequestsPerMinute);