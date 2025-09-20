using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using UserService.Repositories.Entities;
using UserService.Repositories.Queries;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly UserMapper _mapper;

    public UserRepository(IOptions<DatabaseSettings> options, UserMapper mapper)
    {
        _connectionString = options.Value.ConnectionString;
        _mapper = mapper;
    }

    public async Task<UserModel> CreateUserAsync(CreateUserDto dto)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var userEntity = await connection.QueryFirstAsync<UserEntity>(UserQueries.Insert, dto);

        return _mapper.ToModel(userEntity);
    }

    public async Task<UserModel?> GetUserByIdAsync(int userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(
            UserQueries.SelectById,
            new { Id = userId });

        return userEntity is null ? null : _mapper.ToModel(userEntity);
    }

    public async Task<UserModel?> GetUserByLoginAsync(string login)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(
            UserQueries.SelectByLogin,
            new { Login = login });

        return userEntity is null ? null : _mapper.ToModel(userEntity);
    }

    public async Task<List<UserModel>> GetUsersByNameAsync(string name, string surname)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var userEntities = await connection.QueryAsync<UserEntity>(
            UserQueries.SelectAllByName,
            new { Name = name, Surname = surname });

        return _mapper.ToModel(userEntities).ToList();
    }

    public async Task<UserModel?> UpdateUserAsync(UpdateUserDto dto)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(UserQueries.Update, dto);
        
        return userEntity is null ? null : _mapper.ToModel(userEntity);
    }

    public async Task<int?> DeleteUserAsync(int userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<int?>(UserQueries.DeleteById, new { Id = userId });
    }
}