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

    public async Task<UserModel> AddAsync(CreateUserDto dto, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.Insert,
            parameters: dto,
            cancellationToken: cancellationToken);

        var userEntity = await connection.QueryFirstAsync<UserEntity>(command);

        return _mapper.ToModel(userEntity);
    }

    public async Task<UserModel?> GetByIdAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.SelectById,
            parameters: new { Id = userId },
            cancellationToken: cancellationToken);
        
        var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(command);

        return userEntity is null ? null : _mapper.ToModel(userEntity);
    }

    public async Task<UserModel?> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.SelectByLogin,
            parameters: new { Login = login },
            cancellationToken: cancellationToken);
        
        var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(command);

        return userEntity is null ? null : _mapper.ToModel(userEntity);
    }

    public async Task<UserModel[]> FindByNameAsync(string name, string surname, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.SelectAllByName,
            parameters: new { Name = name, Surname = surname },
            cancellationToken: cancellationToken);
        
        var userEntities = await connection.QueryAsync<UserEntity>(command);

        return _mapper.ToModel(userEntities).ToArray();
    }

    public async Task<UserModel?> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.Update,
            parameters: dto,
            cancellationToken: cancellationToken);
        
        var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(command);

        return userEntity is null ? null : _mapper.ToModel(userEntity);
    }

    public async Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.DeleteById,
            parameters: new { Id = userId },
            cancellationToken: cancellationToken);
        
        return await connection.QueryFirstOrDefaultAsync<int?>(command);
    }
}