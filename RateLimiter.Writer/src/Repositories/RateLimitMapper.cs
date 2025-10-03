using RateLimiter.Writer.Repositories.DbModels;
using RateLimiter.Writer.Services.Entities;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Writer.Repositories;

[Mapper]
public partial class RateLimitMapper
{
    public partial RateLimitModel ToDbModel(RateLimitEntity model);
    
    public partial RateLimitEntity ToEntity(RateLimitModel model);
    
    public partial IEnumerable<RateLimitModel> ToDbModels(IEnumerable<RateLimitEntity> entities);
    
    public partial IEnumerable<RateLimitEntity> ToEntities(IEnumerable<RateLimitModel> entities);
}
