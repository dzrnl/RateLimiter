namespace UserService.Repositories.Queries;

public static class UserQueries
{
    public const string SelectById =
        "CALL get_user_by_id(@p_id, @o_id, @o_login, @o_password, @o_name, @o_surname, @o_age)";

    public const string SelectByLogin =
        "CALL get_user_by_login(@p_login, @o_id, @o_login, @o_password, @o_name, @o_surname, @o_age)";

    public const string SelectAllByName =
        "SELECT * FROM get_users_by_name(@Name, @Surname)";

    public const string Update =
        "CALL update_user(@p_id, @p_password, @p_name, @p_surname, @p_age, @o_id, @o_login, @o_password, @o_name, @o_surname, @o_age)";

    public const string Insert =
        "CALL insert_user(@p_login, @p_password, @p_name, @p_surname, @p_age, @o_id, @o_login, @o_password, @o_name, @o_surname, @o_age)";

    public const string DeleteById =
        "CALL delete_user(@p_id, @o_id)";
}