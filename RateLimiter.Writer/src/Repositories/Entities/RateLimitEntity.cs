using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Writer.Repositories.Entities;

[BsonIgnoreExtraElements]
public class RateLimitEntity(string route, int requestsPerMinute)
{
    public string Route { get; set; } = route;

    public int RequestsPerMinute { get; set; } = requestsPerMinute;
}