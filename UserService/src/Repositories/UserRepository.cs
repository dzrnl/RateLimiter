using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using UserService.Repositories.Queries;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IOptions<DatabaseSettings> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    public async Task<UserModel> CreateUserAsync(CreateUserDto dto)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var userId = await connection.ExecuteScalarAsync<int>(UserQueries.Insert, dto);

        return await GetUserByIdAsync(userId); // TODO
    }

    public async Task<UserModel?> GetUserByIdAsync(int userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<UserModel>( // TODO: entity
            UserQueries.SelectById,
            new { Id = userId });
    }

    public Task<UserModel?> GetUserByLoginAsync(string login)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel?> GetUserByNameAsync(string name, string surname)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<UserModel>(
            UserQueries.SelectByName,
            new { Name = name, Surname = surname });
    }

    public async Task<int> UpdateUserAsync(UpdateUserDto dto)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new
        {
            dto.Id,
            dto.Password,
            dto.Name,
            dto.Surname,
            dto.Age
        };

        return await connection.ExecuteScalarAsync<int>(UserQueries.Update, parameters);
    }

    public async Task<int?> DeleteUserAsync(int userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.ExecuteScalarAsync<int>(UserQueries.DeleteById, new { Id = userId });
    }
}