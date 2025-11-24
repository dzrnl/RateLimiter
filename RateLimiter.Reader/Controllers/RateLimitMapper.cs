using RateLimiter.Reader.Services.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Reader.Controllers;

[Mapper]
public partial class RateLimitMapper
{
    public partial RateLimitResponse ToResponse(RateLimit model);

    public RateLimitListResponse ToListResponse(IReadOnlyCollection<RateLimit> models)
        => new()
        {
            Limits = { models.Select(ToResponse) }
        };
}