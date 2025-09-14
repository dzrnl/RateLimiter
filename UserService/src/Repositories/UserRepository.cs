using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using UserService.Services.Dtos;
using UserService.Services.Models;


namespace UserService.Repositories; 

class UserRepository : IUserRepository {
    private static readonly string _selectQuery = $@"select
        id as {nameof(User.Id)},
        login as {nameof(User.Login)},
        password as {nameof(User.Password)},
        name as {nameof(User.Name)},
        surname as {nameof(User.Surname)},
        age as {nameof(User.Age)}
        from users";

    private static readonly string _selectByIdQuery = $@"select
        id as {nameof(User.Id)},
        login as {nameof(User.Login)},
        password as {nameof(User.Password)},
        name as {nameof(User.Name)},
        surname as {nameof(User.Surname)},
        age as {nameof(User.Age)}
        from users
        where id = @Id";

    private static readonly string _selectByNameQuery = $@"select
        id as {nameof(User.Id)},
        login as {nameof(User.Login)},
        password as {nameof(User.Password)},
        name as {nameof(User.Name)},
        surname as {nameof(User.Surname)},
        age as {nameof(User.Age)}
        from users
        where name = @Name";

    private static readonly string _updateQuery = $@"UPDATE users 
        SET password = @Password, name = @Name, 
        surname = @Surname, age = @Age
        WHERE id = @Id
        RETURNING id as {nameof(User.Id)},
        login as {nameof(User.Login)},
        password as {nameof(User.Password)},
        name as {nameof(User.Name)},
        surname as {nameof(User.Surname)},
        age as {nameof(User.Age)}";

    private static readonly string _insertQuery = $@"INSERT INTO users 
        (login, password, name, surname, age) 
        VALUES (@Login, @Password, @Name, @Surname, @Age)
        RETURNING id";

    private static readonly string _deleteByIdQuery = $@"DELETE 
        FROM users 
        WHERE id = @Id";

    private static readonly string _existsQuery = "SELECT COUNT(1) FROM users WHERE id = @Id";

    private readonly string _connectionString;

    UserRepository(string dbConnection) {
        _connectionString = dbConnection;
    }

    public Task<int> CreateUserAsync(CreateUserDto user) {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<int>(_insertQuery, user);
            }
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<User>(
                _selectByIdQuery, 
                new { Id = id });
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryAsync<User>(_selectQuery).ToList();
        }
    }

    public async Task<User> GetUserByNameAsync(string name)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<User>(
                _selectByNameQuery, 
                new { Name = name });
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(_deleteByIdQuery, new { Id = id });
        }
    }

    public async Task<bool> ExistsUserAsync(int id)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var count = await connection.ExecuteScalarAsync<int>(_existsQuery, new { Id = id });
            return count > 0;
        }
    }

    public async Task<User> UpdateUserAsync(int id, UpdateUserDto updatableFields)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var parameters = new
            {
                Id = id,
                updatableFields.Password,
                updatableFields.Name,
                updatableFields.Surname,
                updatableFields.Age
            };
            
            return await connection.QueryFirstOrDefaultAsync<User>(_updateQuery, parameters);
        }
    }
}
