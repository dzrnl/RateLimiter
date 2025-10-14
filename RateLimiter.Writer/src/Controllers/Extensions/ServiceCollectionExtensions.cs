using FluentValidation;
using RateLimiter.Writer.Controllers.Validators;

namespace RateLimiter.Writer.Controllers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection collection)
    {
        collection.AddSingleton<RateLimitMapper>();

        collection.AddSingleton<IValidator<CreateRateLimitRequest>, CreateRateLimitRequestValidator>();
        collection.AddSingleton<IValidator<UpdateRateLimitRequest>, UpdateRateLimitRequestValidator>();

        return collection;
    }
}