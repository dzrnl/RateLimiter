using FluentValidation;
using UserService.Controllers.Validators;

namespace UserService.Controllers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection collection)
    {
        collection.AddSingleton<UserMapper>();

        collection.AddSingleton<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        collection.AddSingleton<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();

        return collection;
    }
}