namespace UserService.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDataAccess(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));

        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddSingleton<UserMapper>();

        return collection;
    }
}