using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using UserService.Repositories.Configuration;
using UserService.Repositories.Entities;
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

    public async Task<IUserModel> AddAsync(ICreateUserDto dto, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_login", dto.Login, DbType.String);
        parameters.Add("p_password", dto.Password, DbType.String);
        parameters.Add("p_name", dto.Name, DbType.String);
        parameters.Add("p_surname", dto.Surname, DbType.String);
        parameters.Add("p_age", dto.Age, DbType.Int32);

        var command = new CommandDefinition(
            commandText: UserQueries.Insert,
            parameters: parameters,
            cancellationToken: cancellationToken);

        var entity = await connection.QuerySingleAsync<UserEntity>(command);

        return entity;
    }

    public async Task<IUserModel?> FindByIdAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_id", userId, DbType.Int32);

        var command = new CommandDefinition(
            UserQueries.SelectById,
            parameters,
            cancellationToken: cancellationToken);
        
        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command);

        return entity;
    }

    public async Task<IUserModel?> FindByLoginAsync(string login, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_login", login, DbType.String);

        var command = new CommandDefinition(
            UserQueries.SelectByLogin,
            parameters,
            cancellationToken: cancellationToken);

        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command);

        return entity;
    }

    public async Task<IUserModel[]> FindAllByNameAsync(string name, string surname, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_name", name, DbType.String);
        parameters.Add("p_surname", surname, DbType.String);

        var command = new CommandDefinition(
            UserQueries.SelectAllByName,
            parameters,
            cancellationToken: cancellationToken);

        var entities = await connection.QueryAsync<UserEntity>(command);

        return entities.ToArray<IUserModel>();
    }

    public async Task<IUserModel?> UpdateAsync(IUpdateUserDto dto, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_id", dto.Id, DbType.Int32);
        parameters.Add("p_password", dto.Password, DbType.String);
        parameters.Add("p_name", dto.Name, DbType.String);
        parameters.Add("p_surname", dto.Surname, DbType.String);
        parameters.Add("p_age", dto.Age, DbType.Int32);

        var command = new CommandDefinition(
            UserQueries.Update,
            parameters,
            cancellationToken: cancellationToken);

        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command);

        return entity;
    }

    public async Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_id", userId, DbType.Int32);

        var command = new CommandDefinition(
            UserQueries.DeleteById,
            parameters,
            cancellationToken: cancellationToken);

        var deletedId = await connection.QueryFirstOrDefaultAsync<int>(command);

        return deletedId;
    }
}