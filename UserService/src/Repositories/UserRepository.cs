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
    private readonly UserMapper _mapper;

    public UserRepository(IOptions<DatabaseSettings> options, UserMapper mapper)
    {
        _connectionString = options.Value.ConnectionString;
        _mapper = mapper;
    }

    public async Task<UserModel> AddAsync(CreateUserDto dto, CancellationToken cancellationToken)
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

        return _mapper.ToModel(entity);
    }

    public async Task<UserModel?> FindByIdAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_id", userId, DbType.Int32);

        var command = new CommandDefinition(
            UserQueries.SelectById,
            parameters,
            cancellationToken: cancellationToken);
        
        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command);

        return entity == null ? null : _mapper.ToModel(entity);
    }

    public async Task<UserModel?> FindByLoginAsync(string login, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_login", login, DbType.String);

        var command = new CommandDefinition(
            UserQueries.SelectByLogin,
            parameters,
            cancellationToken: cancellationToken);

        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command);

        return entity == null ? null : _mapper.ToModel(entity);
    }

    public async Task<UserModel[]> FindAllByNameAsync(string name, string surname, CancellationToken cancellationToken)
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

        return _mapper.ToModels(entities).ToArray();
    }

    public async Task<UserModel?> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken)
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

        return entity == null ? null : _mapper.ToModel(entity);
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