using UserService.Repositories.Entities;

namespace UserService.Repositories.Queries;

public static class UserQueries
{
    private const string TableName = "users";

    private const string Columns = $"""
                                    id AS {nameof(UserEntity.Id)},
                                    login AS {nameof(UserEntity.Login)},
                                    password AS {nameof(UserEntity.Password)},
                                    name AS {nameof(UserEntity.Name)},
                                    surname AS {nameof(UserEntity.Surname)},
                                    age AS {nameof(UserEntity.Age)}
                                    """;

    private const string IdColumn = $"id AS {nameof(UserEntity.Id)}";

    public const string SelectById = $"""
                                      SELECT {Columns}
                                      FROM {TableName}
                                      WHERE id = @Id
                                      """;

    public const string SelectByLogin = $"""
                                         SELECT {Columns}
                                         FROM {TableName}
                                         WHERE login = @Login
                                         """;

    public const string SelectAllByName = $"""
                                           SELECT {Columns}
                                           FROM {TableName}
                                           WHERE name = @Name AND surname = @Surname
                                           """;

    public const string Update = $"""
                                  UPDATE {TableName} 
                                  SET password = COALESCE(@Password, password),
                                      name = COALESCE(@Name, name),
                                      surname = COALESCE(@Surname, surname),
                                      age = COALESCE(@Age, age)
                                  WHERE id = @Id
                                  RETURNING {Columns}
                                  """;

    public const string Insert = $"""
                                  INSERT INTO {TableName} 
                                      (login, password, name, surname, age) 
                                  VALUES (@Login, @Password, @Name, @Surname, @Age)
                                  RETURNING {Columns}
                                  """;

    public const string DeleteById = $"""
                                      DELETE FROM {TableName}
                                      WHERE id = @Id
                                      RETURNING {IdColumn}
                                      """;
}