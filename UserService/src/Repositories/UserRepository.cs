using Dapper;
using Npgsql;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private const string SelectQuery = $"""
                                        select
                                                id as {nameof(UserModel.Id)},
                                                login as {nameof(UserModel.Login)},
                                                password as {nameof(UserModel.Password)},
                                                name as {nameof(UserModel.Name)},
                                                surname as {nameof(UserModel.Surname)},
                                                age as {nameof(UserModel.Age)}
                                                from users
                                        """;

    private const string SelectByIdQuery = $"""
                                            select
                                                    id as {nameof(UserModel.Id)},
                                                    login as {nameof(UserModel.Login)},
                                                    password as {nameof(UserModel.Password)},
                                                    name as {nameof(UserModel.Name)},
                                                    surname as {nameof(UserModel.Surname)},
                                                    age as {nameof(UserModel.Age)}
                                                    from users
                                                    where id = @Id
                                            """;

    private const string SelectByNameQuery = $"""
                                              select
                                                      id as {nameof(UserModel.Id)},
                                                      login as {nameof(UserModel.Login)},
                                                      password as {nameof(UserModel.Password)},
                                                      name as {nameof(UserModel.Name)},
                                                      surname as {nameof(UserModel.Surname)},
                                                      age as {nameof(UserModel.Age)}
                                                      from users
                                                      where name = @Name and surname = @Surname LIMIT 1
                                              """;

    private const string UpdateQuery = $"""
                                        UPDATE users 
                                                SET password = @Password, name = @Name, 
                                                surname = @Surname, age = @Age
                                                WHERE id = @Id
                                                RETURNING id as {nameof(UserModel.Id)},
                                                login as {nameof(UserModel.Login)},
                                                password as {nameof(UserModel.Password)},
                                                name as {nameof(UserModel.Name)},
                                                surname as {nameof(UserModel.Surname)},
                                                age as {nameof(UserModel.Age)}
                                        """;

    private const string InsertQuery = """
                                       INSERT INTO users 
                                               (login, password, name, surname, age) 
                                               VALUES (@Login, @Password, @Name, @Surname, @Age)
                                               RETURNING id
                                       """;

    private const string DeleteByIdQuery = """
                                           DELETE 
                                                   FROM users 
                                                   WHERE id = @Id
                                           """;

    private const string ExistsQuery = "SELECT COUNT(1) FROM users WHERE id = @Id";

    private const string ConnectionString = "";

    public async Task<int> CreateUserAsync(CreateUserDto user)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        return await connection.ExecuteScalarAsync<int>(InsertQuery, user);
    }

    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        return await connection.QueryFirstOrDefaultAsync<UserModel>(
            SelectByIdQuery,
            new { Id = id });
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var users = await connection.QueryAsync<UserModel>(SelectQuery);
        return users.ToList();
    }

    public async Task<UserModel> GetUserByNameAsync(string name, string surname)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        return await connection.QueryFirstOrDefaultAsync<UserModel>(
            SelectByNameQuery,
            new { Name = name, Surname = surname });
    }

    public async Task DeleteUserAsync(int id)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        await connection.ExecuteAsync(DeleteByIdQuery, new { Id = id });
    }

    public async Task<bool> ExistsUserAsync(int id)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        var count = await connection.ExecuteScalarAsync<int>(ExistsQuery, new { Id = id });
        return count > 0;
    }

    public async Task<UserModel> UpdateUserAsync(UpdateUserDto updatableFields)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var parameters = new
        {
            updatableFields.Id,
            updatableFields.Password,
            updatableFields.Name,
            updatableFields.Surname,
            updatableFields.Age
        };

        return await connection.QueryFirstOrDefaultAsync<UserModel>(UpdateQuery, parameters);
    }
}