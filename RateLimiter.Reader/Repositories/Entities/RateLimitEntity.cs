using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RateLimiter.Reader.Repositories.Entities;

[BsonIgnoreExtraElements]
public record RateLimitEntity(
    [property: BsonId]
    [property: BsonRepresentation(BsonType.String)]
    string Route,
    int RequestsPerMinute
);