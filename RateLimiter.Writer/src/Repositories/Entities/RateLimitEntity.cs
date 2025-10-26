using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Writer.Repositories.Entities;

[BsonIgnoreExtraElements]
public record RateLimitEntity(string Route, int RequestsPerMinute);