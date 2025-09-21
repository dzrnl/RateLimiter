using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
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

        var p = new DynamicParameters();
        p.Add("p_login", dto.Login);
        p.Add("p_password", dto.Password);
        p.Add("p_name", dto.Name);
        p.Add("p_surname", dto.Surname);
        p.Add("p_age", dto.Age);

        p.Add("o_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("o_login", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_password", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_name", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_surname", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_age", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            new CommandDefinition(UserQueries.Insert, 
            p, cancellationToken: cancellationToken));

        var entity = new UserEntity
        {
            Id = p.Get<int>("o_id"),
            Login = p.Get<string>("o_login"),
            Password = p.Get<string>("o_password"),
            Name = p.Get<string>("o_name"),
            Surname = p.Get<string>("o_surname"),
            Age = p.Get<int>("o_age")
        };

        return _mapper.ToModel(entity);
    }

    public async Task<UserModel?> FindByIdAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var p = new DynamicParameters();
        p.Add("p_id", userId);
        p.Add("o_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("o_login", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_password", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_name", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_surname", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_age", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            new CommandDefinition(UserQueries.SelectById, 
            p, cancellationToken: cancellationToken));

        if (p.Get<int?>("o_id") is null)
            return null;

        var entity = new UserEntity
        {
            Id = p.Get<int>("o_id"),
            Login = p.Get<string>("o_login"),
            Password = p.Get<string>("o_password"),
            Name = p.Get<string>("o_name"),
            Surname = p.Get<string>("o_surname"),
            Age = p.Get<int>("o_age")
        };

        return _mapper.ToModel(entity);
    }

    public async Task<UserModel?> FindByLoginAsync(string login, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var p = new DynamicParameters();
        p.Add("p_login", login);
        p.Add("o_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("o_login", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_password", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_name", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_surname", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_age", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            new CommandDefinition(UserQueries.SelectByLogin, 
            p, cancellationToken: cancellationToken));

        if (p.Get<int?>("o_id") is null)
            return null;

        var entity = new UserEntity
        {
            Id = p.Get<int>("o_id"),
            Login = p.Get<string>("o_login"),
            Password = p.Get<string>("o_password"),
            Name = p.Get<string>("o_name"),
            Surname = p.Get<string>("o_surname"),
            Age = p.Get<int>("o_age")
        };

        return _mapper.ToModel(entity);
    }

    public async Task<UserModel[]> FindAllByNameAsync(string name, string surname, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var command = new CommandDefinition(
            UserQueries.SelectAllByName,
            parameters: new { Name = name, Surname = surname },
            cancellationToken: cancellationToken);
        
        var userEntities = await connection.QueryAsync<UserEntity>(command);

        return _mapper.ToModels(userEntities).ToArray();
    }

    public async Task<UserModel?> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var p = new DynamicParameters();
        p.Add("p_id", dto.Id);
        p.Add("p_password", dto.Password);
        p.Add("p_name", dto.Name);
        p.Add("p_surname", dto.Surname);
        p.Add("p_age", dto.Age);

        p.Add("o_id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("o_login", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_password", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_name", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_surname", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
        p.Add("o_age", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            new CommandDefinition(UserQueries.Update, 
            p, cancellationToken: cancellationToken));

        if (p.Get<int?>("o_id") is null)
            return null;

        var entity = new UserEntity
        {
            Id = p.Get<int>("o_id"),
            Login = p.Get<string>("o_login"),
            Password = p.Get<string>("o_password"),
            Name = p.Get<string>("o_name"),
            Surname = p.Get<string>("o_surname"),
            Age = p.Get<int>("o_age")
        };

        return _mapper.ToModel(entity);
    }

    public async Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var p = new DynamicParameters();
        p.Add("p_id", userId);
        p.Add("o_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            new CommandDefinition(UserQueries.DeleteById, 
            p, cancellationToken: cancellationToken));

        return p.Get<int?>("o_id");
    }
}