/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE [USER] (
            username NVARCHAR(30) PRIMARY KEY,
            encrypted_password TEXT,
            email NVARCHAR(30)
        );

        CREATE TABLE NOTE (
            note_id INT IDENTITY(1,1) PRIMARY KEY,
            username NVARCHAR(30) FOREIGN KEY REFERENCES [USER](username) ON DELETE NO ACTION,
            name NVARCHAR(50),
            content TEXT
        );

        CREATE TABLE TASK (
            task_id INT IDENTITY(1,1) PRIMARY KEY,
            username NVARCHAR(30) FOREIGN KEY REFERENCES [USER](username) ON DELETE NO ACTION,
            name NVARCHAR(50),
            due_date DATETIME,
            completed BIT,
            repeat_option NVARCHAR(30),
            description NVARCHAR(100),
            note_id INT NULL,  -- Allow NULL
            FOREIGN KEY (note_id) REFERENCES NOTE(note_id) ON DELETE SET NULL, 
            reminder DATETIME,
            important BIT
        );

        CREATE TABLE FOCUS_SESSION (
            session_id INT IDENTITY(1,1) PRIMARY KEY,
            username NVARCHAR(30) FOREIGN KEY REFERENCES [USER](username) ON DELETE NO ACTION,
            duration INT,  -- Store duration as seconds
            tag NVARCHAR(30),
            timestamp DATETIME
        );
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE FOCUS_SESSION;
        DROP TABLE TASK;
        DROP TABLE NOTE;
        DROP TABLE [USER];
    `);
};
