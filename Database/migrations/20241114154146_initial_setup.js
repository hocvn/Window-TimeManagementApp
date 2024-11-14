/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE [USER] (
            user_id INT IDENTITY(1,1) PRIMARY KEY,
            username VARCHAR(50),
            encrypted_password VARCHAR(50),
            email VARCHAR(50)
        );

        CREATE TABLE NOTE (
            note_id INT IDENTITY(1,1) PRIMARY KEY,
            user_id INT FOREIGN KEY REFERENCES [USER](user_id) ON DELETE NO ACTION,
            name NVARCHAR(50),
            content TEXT
        );

        CREATE TABLE TASK (
            task_id INT IDENTITY(1,1) PRIMARY KEY,
            user_id INT FOREIGN KEY REFERENCES [USER](user_id) ON DELETE NO ACTION,
            name NVARCHAR(50),
            due_date DATETIME,
            completed BIT,
            repeat_option VARCHAR(50),
            description NVARCHAR(100),
            note_id INT NULL,  -- Cho phép NULL
            FOREIGN KEY (note_id) REFERENCES NOTE(note_id) ON DELETE SET NULL, 
            reminder DATETIME,
            important BIT
        );


        CREATE TABLE FOCUS_SESSION (
            session_id INT IDENTITY(1,1) PRIMARY KEY,
            user_id INT FOREIGN KEY REFERENCES [USER](user_id) ON DELETE NO ACTION,
            timespan BIGINT,  
            tag VARCHAR(50),
            date DATE
        );
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE FOCUS_SESSION
        DROP TABLE TASK
        DROP TABLE NOTE
        DROP TABLE [USER]
    `);
};
