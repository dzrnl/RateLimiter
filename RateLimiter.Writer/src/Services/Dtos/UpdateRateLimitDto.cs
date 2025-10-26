namespace RateLimiter.Writer.Services.Dtos;

public sealed record UpdateRateLimitDto(string Route, int RequestsPerMinute);