namespace RateLimiter.Writer.Services.Dtos;

public sealed record CreateRateLimitDto(string Route, int RequestsPerMinute);