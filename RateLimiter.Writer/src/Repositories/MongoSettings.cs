namespace RateLimiter.Writer.Repositories;

public class MongoSettings
{
    public string MongoUri { get; set; } = string.Empty;
    public string MongoDatabase { get; set; } = string.Empty;
}