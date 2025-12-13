using FluentValidation;

namespace RateLimiter.Writer.Controllers.Validators;

public class CreateRateLimitRequestValidator : AbstractValidator<CreateRateLimitRequest>
{
    public CreateRateLimitRequestValidator()
    {
        RuleFor(x => x.RequestsPerMinute)
            .GreaterThan(0).WithMessage("Requests per minute must be greater than 0");
    }
}