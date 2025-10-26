using FluentValidation;

namespace RateLimiter.Writer.Controllers.Validators;

public class UpdateRateLimitRequestValidator : AbstractValidator<UpdateRateLimitRequest>
{
    public UpdateRateLimitRequestValidator()
    {
        RuleFor(x => x.Route)
            .NotEmpty().WithMessage("Route is required");

        RuleFor(x => x.RequestsPerMinute)
            .GreaterThan(0).WithMessage("Requests per minute must be greater than 0");
    }
}