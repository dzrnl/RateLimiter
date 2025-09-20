using UserService.Repositories.Entities;

namespace UserService.Repositories.Queries;

public static class UserQueries
{
    public const string Select = $"""
                                  select
                                          id as {nameof(UserEntity.Id)},
                                          login as {nameof(UserEntity.Login)},
                                          password as {nameof(UserEntity.Password)},
                                          name as {nameof(UserEntity.Name)},
                                          surname as {nameof(UserEntity.Surname)},
                                          age as {nameof(UserEntity.Age)}
                                          from users
                                  """;

    public const string SelectById = $"""
                                      select
                                              id as {nameof(UserEntity.Id)},
                                              login as {nameof(UserEntity.Login)},
                                              password as {nameof(UserEntity.Password)},
                                              name as {nameof(UserEntity.Name)},
                                              surname as {nameof(UserEntity.Surname)},
                                              age as {nameof(UserEntity.Age)}
                                              from users
                                              where id = @Id
                                      """;

    public const string SelectByName = $"""
                                        select
                                                id as {nameof(UserEntity.Id)},
                                                login as {nameof(UserEntity.Login)},
                                                password as {nameof(UserEntity.Password)},
                                                name as {nameof(UserEntity.Name)},
                                                surname as {nameof(UserEntity.Surname)},
                                                age as {nameof(UserEntity.Age)}
                                                from users
                                                where name = @Name and surname = @Surname LIMIT 1
                                        """;

    public const string Update = $"""
                                  UPDATE users 
                                          SET password = @Password, name = @Name, 
                                          surname = @Surname, age = @Age
                                          WHERE id = @Id
                                          RETURNING id as {nameof(UserEntity.Id)},
                                          login as {nameof(UserEntity.Login)},
                                          password as {nameof(UserEntity.Password)},
                                          name as {nameof(UserEntity.Name)},
                                          surname as {nameof(UserEntity.Surname)},
                                          age as {nameof(UserEntity.Age)}
                                  """;

    public const string Insert = """
                                 INSERT INTO users 
                                         (login, password, name, surname, age) 
                                         VALUES (@Login, @Password, @Name, @Surname, @Age)
                                         RETURNING id
                                 """;

    public const string DeleteById = """
                                     DELETE 
                                             FROM users 
                                             WHERE id = @Id
                                             RETURNING id
                                     """;
}