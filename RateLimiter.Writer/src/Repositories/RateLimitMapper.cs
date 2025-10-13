using RateLimiter.Writer.Repositories.Entities;
using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Writer.Repositories;

[Mapper]
public partial class RateLimitMapper
{
    public partial RateLimit ToModel(RateLimitEntity entity);

    public partial RateLimitEntity ToEntity(CreateRateLimitDto dto);
    
    public partial RateLimitEntity ToEntity(UpdateRateLimitDto dto);

    public partial IEnumerable<RateLimit> ToModels(IEnumerable<RateLimitEntity> entities);

    public partial IEnumerable<RateLimitEntity> ToEntities(IEnumerable<RateLimit> models);
}