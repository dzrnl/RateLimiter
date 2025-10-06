using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Writer.Repositories.Entities;

[BsonIgnoreExtraElements]
public class RateLimitEntity(string route, int requestsPerMinute)
{
    [BsonElement("route")] 
    public string Route { get; set; } = route;

    [BsonElement("requests_per_minute")] 
    public int RequestsPerMinute { get; set; } = requestsPerMinute;
}