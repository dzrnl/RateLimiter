CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    login VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    name VARCHAR(255) NOT NULL,
    surname VARCHAR(255) NOT NULL,
    age INT NOT NULL
);


CREATE INDEX id_idx on users USING HASH(id);
CREATE INDEX name_surname_idx on users USING BTREE(name, surname);
CREATE INDEX login_idx on users(login);


CREATE OR REPLACE FUNCTION insert_user_fn(
    p_login VARCHAR,
    p_password VARCHAR,
    p_name VARCHAR,
    p_surname VARCHAR,
    p_age INT
)
RETURNS TABLE (
    id INT,
    login VARCHAR,
    password VARCHAR,
    name VARCHAR,
    surname VARCHAR,
    age INT
) AS $$
BEGIN
    RETURN QUERY
    INSERT INTO users (login, password, name, surname, age)
    VALUES (p_login, p_password, p_name, p_surname, p_age)
    RETURNING users.id, users.login, users.password, users.name, users.surname, users.age;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION get_user_by_id_fn(
    p_id INT
)
RETURNS TABLE (
    id INT,
    login VARCHAR,
    password VARCHAR,
    name VARCHAR,
    surname VARCHAR,
    age INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.id, u.login, u.password, u.name, u.surname, u.age
    FROM users u
    WHERE u.id = p_id;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION get_user_by_login_fn(
    p_login VARCHAR
)
RETURNS TABLE (
    id INT,
    login VARCHAR,
    password VARCHAR,
    name VARCHAR,
    surname VARCHAR,
    age INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.id, u.login, u.password, u.name, u.surname, u.age
    FROM users u
    WHERE u.login = p_login;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION get_users_by_name_fn(
    p_name VARCHAR,
    p_surname VARCHAR
)
RETURNS TABLE (
    id INT,
    login VARCHAR,
    password VARCHAR,
    name VARCHAR,
    surname VARCHAR,
    age INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.id, u.login, u.password, u.name, u.surname, u.age
    FROM users u
    WHERE u.name = p_name AND u.surname = p_surname;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION update_user_fn(
    p_id INT,
    p_password VARCHAR,
    p_name VARCHAR,
    p_surname VARCHAR,
    p_age INT
)
RETURNS TABLE (
    id INT,
    login VARCHAR,
    password VARCHAR,
    name VARCHAR,
    surname VARCHAR,
    age INT
) AS $$
BEGIN
    RETURN QUERY
    UPDATE users u
    SET password = COALESCE(p_password, u.password),
        name = COALESCE(p_name, u.name),
        surname = COALESCE(p_surname, u.surname),
        age = COALESCE(p_age, u.age)
    WHERE u.id = p_id
    RETURNING u.id, u.login, u.password, u.name, u.surname, u.age;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION delete_user_fn(
    p_id INT
)
RETURNS TABLE (
    id INT
) AS $$
BEGIN
    RETURN QUERY
    DELETE FROM users u
    WHERE u.id = p_id
    RETURNING u.id;
END;
$$ LANGUAGE plpgsql;