using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Writer.Repositories.Entities;

[BsonIgnoreExtraElements]
public record RateLimitEntity(
    [property: BsonId] string Route,
    int RequestsPerMinute
);