namespace UserService.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(this IServiceCollection collection)
    {
        collection.AddScoped<IUserRepository, UserRepository>();
        return collection;
    }
}