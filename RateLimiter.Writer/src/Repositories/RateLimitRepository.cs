using RateLimiter.Writer.Repositories.DbModels;
using RateLimiter.Writer.Services.Entities;

namespace RateLimiter.Writer.Repositories;

public class RateLimitRepository
{
    private readonly RateLimitMapper _mapper;
    
    public RateLimitRepository(RateLimitMapper mapper)
    {
        this._mapper = mapper;
    }
    
    public string CreateRateLimit(RateLimitModel model)
    {
        RateLimitEntity entity = this._mapper.ToEntity(model);
        // TODO
    }
}