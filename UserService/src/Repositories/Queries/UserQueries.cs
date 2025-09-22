namespace UserService.Repositories.Queries;

public static class UserQueries
{
    public const string SelectById =
        "SELECT * FROM get_user_by_id_fn(@p_id)";

    public const string SelectByLogin =
        "SELECT * FROM get_user_by_login_fn(@p_login)";

    public const string SelectAllByName =
        "SELECT * FROM get_users_by_name_fn(@p_name, @p_surname)";

    public const string Update =
        "SELECT * FROM update_user_fn(@p_id, @p_password, @p_name, @p_surname, @p_age)";

    public const string Insert =
        "SELECT * FROM insert_user_fn(@p_login, @p_password, @p_name, @p_surname, @p_age)";

    public const string DeleteById =
        "SELECT * FROM delete_user_fn(@p_id)";
}