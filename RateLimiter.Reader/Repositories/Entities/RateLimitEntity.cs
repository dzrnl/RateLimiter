using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Reader.Repositories.Entities;

[BsonIgnoreExtraElements]
public record RateLimitEntity(string Route, int RequestsPerMinute);