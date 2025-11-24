namespace RateLimiter.Reader.Repositories;

public class DatabaseSettings
{
    public string Uri { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public string CollectionName { get; set; } = string.Empty;

    public int BatchSize { get; set; }
}