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
