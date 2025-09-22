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


CREATE OR REPLACE PROCEDURE insert_user(
    IN p_login VARCHAR,
    IN p_password VARCHAR,
    IN p_name VARCHAR,
    IN p_surname VARCHAR,
    IN p_age INT,
    OUT o_id INT,
    OUT o_login VARCHAR,
    OUT o_password VARCHAR,
    OUT o_name VARCHAR,
    OUT o_surname VARCHAR,
    OUT o_age INT
)
LANGUAGE plpgsql AS $$
BEGIN
INSERT INTO users (login, password, name, surname, age)
VALUES (p_login, p_password, p_name, p_surname, p_age)
    RETURNING id, login, password, name, surname, age
INTO o_id, o_login, o_password, o_name, o_surname, o_age;
END;
$$;


CREATE OR REPLACE PROCEDURE get_user_by_id(
    IN p_id INT,
    OUT o_id INT,
    OUT o_login VARCHAR,
    OUT o_password VARCHAR,
    OUT o_name VARCHAR,
    OUT o_surname VARCHAR,
    OUT o_age INT
)
LANGUAGE plpgsql AS $$
BEGIN
    SELECT id, login, password, name, surname, age
    INTO o_id, o_login, o_password, o_name, o_surname, o_age
    FROM users
    WHERE id = p_id;
END;
$$;


CREATE OR REPLACE PROCEDURE get_user_by_login(
    IN p_login VARCHAR,
    OUT o_id INT,
    OUT o_login VARCHAR,
    OUT o_password VARCHAR,
    OUT o_name VARCHAR,
    OUT o_surname VARCHAR,
    OUT o_age INT
)
LANGUAGE plpgsql AS $$
BEGIN
    SELECT id, login, password, name, surname, age
    INTO o_id, o_login, o_password, o_name, o_surname, o_age
    FROM users
    WHERE login = p_login;
END;
$$;


CREATE OR REPLACE FUNCTION get_users_by_name(
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


CREATE OR REPLACE PROCEDURE update_user(
    IN p_id INT,
    IN p_password VARCHAR,
    IN p_name VARCHAR,
    IN p_surname VARCHAR,
    IN p_age INT,
    OUT o_id INT,
    OUT o_login VARCHAR,
    OUT o_password VARCHAR,
    OUT o_name VARCHAR,
    OUT o_surname VARCHAR,
    OUT o_age INT
)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE users
    SET password = COALESCE(p_password, password),
        name = COALESCE(p_name, name),
        surname = COALESCE(p_surname, surname),
        age = COALESCE(p_age, age)
    WHERE id = p_id
    RETURNING id, login, password, name, surname, age
    INTO o_id, o_login, o_password, o_name, o_surname, o_age;
END;
$$;


CREATE OR REPLACE PROCEDURE delete_user(
    IN p_id INT,
    OUT o_id INT
)
LANGUAGE plpgsql AS $$
BEGIN
    DELETE FROM users
    WHERE id = p_id
    RETURNING id INTO o_id;
END;
$$;