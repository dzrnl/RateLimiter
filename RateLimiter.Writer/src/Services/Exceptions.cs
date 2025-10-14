namespace RateLimiter.Writer.Services;

public class RateLimitAlreadyExistsException(string route)
    : Exception($"Rate limit for route '{route}' already exists");

public class RateLimitNotFoundException(string route)
    : Exception($"Rate limit for route '{route}' was not found");