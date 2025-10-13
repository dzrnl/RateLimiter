using RateLimiter.Writer.Services.Dtos;
using RateLimiter.Writer.Services.Models;
using Riok.Mapperly.Abstractions;

namespace RateLimiter.Writer.Controllers;

[Mapper]
public partial class RateLimitMapper
{
    public partial RateLimitResponse ToResponse(RateLimit model);

    public partial CreateRateLimitDto ToCreateModel(CreateRateLimitRequest request);

    public partial UpdateRateLimitDto ToUpdateModel(UpdateRateLimitRequest request);
}