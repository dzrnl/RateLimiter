using RateLimiter.Reader.Repositories.Entities;
using RateLimiter.Reader.Services.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Reader.Repositories;

[Mapper]
public partial class RateLimitMapper
{
    public partial RateLimit ToModel(RateLimitEntity entity);
}