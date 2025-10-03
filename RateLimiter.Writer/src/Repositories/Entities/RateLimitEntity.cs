namespace RateLimiter.Writer.Repositories.DbModels;

public record RateLimitEntity(string Route, int RequestsPerMinute);