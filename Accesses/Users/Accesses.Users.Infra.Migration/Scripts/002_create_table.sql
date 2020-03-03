CREATE TABLE IF NOT EXISTS users.client
(
    id          uuid         not null,
    active      boolean      not null,
    name        varchar(120) not null,
    address     uuid         not null
);

CREATE UNIQUE INDEX client_id_uindex ON users.client (id);

ALTER TABLE users.client
    ADD CONSTRAINT client_pk PRIMARY KEY (id);
