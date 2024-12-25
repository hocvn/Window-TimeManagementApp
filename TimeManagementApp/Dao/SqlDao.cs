using Microsoft.Data.SqlClient;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.Services;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TimeManagementApp.Dao
{
    public class SqlDao : IDao
    {
        public SqlDao() { }

        private readonly UserSingleton User = UserSingleton.Instance;

        private readonly RSAEncryptionService EncryptionService = new();

        private SqlConnection CreateConnection()
        {
            // Store connection string in appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        // Users ------------------------------------------------------------------------------------

        public void CreateUser(string username, string password, string email)
        {
            // Encrypt password and save private key to local settings 
            //(string EncryptedPasswordBase64, RSAParameters PrivateKey) = EncryptionService.Encrypt(password);
            //EncryptionService.SavePrivateKey(PrivateKey, username);
            // Encrypt password by RSA

            string EncryptedPasswordBase64 = EncryptionService.EncryptDPAPI(password);
            var connection = CreateConnection();
            var sql = @"
                        insert into [USER] (username, encrypted_password, email)
                        values (@username, @encrypted_password, @email)
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters.Add("@encrypted_password", System.Data.SqlDbType.Text);
            command.Parameters["@encrypted_password"].Value = EncryptedPasswordBase64;
            command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar);
            command.Parameters["@email"].Value = email;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public bool CheckCredential(string username, string password)
        {
            var connection = CreateConnection();
            var sql = @"
                        select encrypted_password, email
                        from [USER] 
                        where username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;

            var reader = command.ExecuteReader();
            if (reader.Read())
            {

                var encryptedPassword = reader.GetString(0);
                var email = reader.GetString(1);

                // RSA
                //RSAParameters privateKey = EncryptionService.GetPrivateKey(username);
                //string decryptedPassword = EncryptionService.Decrypt(encryptedPassword, privateKey);

                string decryptedPassword = EncryptionService.DecryptDPAPI(encryptedPassword);

                if (password == decryptedPassword)
                {
                    User.Username = username;
                    User.Email = email;
                    User.EncryptedPassword = encryptedPassword;
                    connection.Close();
                    return true;
                }
            }
            connection.Close();
            return false;
        }

        public bool IsUsernameInUse(string username)
        {
            var connection = CreateConnection();
            var sql = @"
                        select email
                        from [USER] 
                        where username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                connection.Close();
                return true;
            }

            connection.Close();
            return false;
        }

        public bool IsEmailInUse(string email)
        {
            var connection = CreateConnection();
            var sql = @"
                        select username
                        from [USER] 
                        where email = @email
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar);
            command.Parameters["@email"].Value = email;

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                connection.Close();
                return true;
            }
            connection.Close();
            return false;

        }

        public string GetUsername(string email)
        {
            var connection = CreateConnection();
            var sql = @"
                        select username
                        from [USER] 
                        where email = @email
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar);
            command.Parameters["@email"].Value = email;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var username = reader.GetString(0);
                connection.Close();
                return username;
            }
            connection.Close();
            return null;
        }

        public string GetPassword(string username)
        {
            var connection = CreateConnection();
            var sql = @"
                        select encrypted_password
                        from [USER] 
                        where username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var encryptedPassword = reader.GetString(0);
                // RSA
                //RSAParameters privateKey = EncryptionService.GetPrivateKey(username);
                //string decryptedPassword = EncryptionService.Decrypt(encryptedPassword, privateKey);

                string decryptedPassword = EncryptionService.DecryptDPAPI(encryptedPassword);
                connection.Close();
                return decryptedPassword;
            }
            connection.Close();
            return null;
        }

        public void ResetPassword(string username, string password, string email)
        {
            // RSA
            // Encrypt new password and save private key to local settings 
            //(string EncryptedPasswordBase64, RSAParameters PrivateKey) = EncryptionService.Encrypt(password);
            //EncryptionService.SavePrivateKey(PrivateKey, username);

            string EncryptedPasswordBase64 = EncryptionService.EncryptDPAPI(password);

            var connection = CreateConnection();
            var sql = @"
                        update [USER] 
                        set encrypted_password = @encrypted_password
                        where username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters.Add("@encrypted_password", System.Data.SqlDbType.Text);
            command.Parameters["@encrypted_password"].Value = EncryptedPasswordBase64;
            command.ExecuteNonQuery();
            connection.Close();
        }


        // Notes ------------------------------------------------------------------------------------
        public ObservableCollection<MyNote> GetAllNote()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyNote>();

            var sql = @"
                        select Note.note_id, Note.name, Note.content
                        from [NOTE] Note
                        where Note.username = @username
                    ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var note = new MyNote
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Content = reader.GetString(2)
                };
                result.Add(note);
            }
            connection.Close();
            return result;
        }

        public void DeleteNote(MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        delete from [NOTE] 
                        where note_id = @note_id and username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public int CreateNote(string noteName)
        {
            var connection = CreateConnection();
            var sql = @"
                insert into [NOTE] (name, content, username)
                values (@name, @content, @username);
                select SCOPE_IDENTITY(); -- Get ID of the newly added record
            ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = noteName;
            command.Parameters.Add("@content", System.Data.SqlDbType.Text);
            // Create an empty RichEditBox to store content of the note
            var editor = new RichEditBox();
            editor.Document.GetText(TextGetOptions.FormatRtf, out string content);
            command.Parameters["@content"].Value = content;

            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            int newNoteId = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();

            return newNoteId;
        }

        public async Task OpenNote(MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        select Note.content
                        from [NOTE] Note
                        where Note.note_id = @note_id and Note.username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                var content = reader.GetString(0);
                note.Content = content;
            }
            connection.Close();
        }

        public void RenameNote(MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        update [NOTE] 
                        set name = @name
                        where note_id = @note_id and username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = note.Name;
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void SaveNote(MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        UPDATE [NOTE] 
                        SET name = @name, content = @content
                        WHERE note_id = @note_id AND username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            // Get content of RichEditBox with RTF format
            command.Parameters.Add("@content", System.Data.SqlDbType.Text);
            command.Parameters["@content"].Value = note.Content;

            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = note.Name;

            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;

            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            command.ExecuteNonQuery();
            connection.Close();
        }


        // Tasks ------------------------------------------------------------------------------------
        // (task_id, username, name, due_date, description, completed, important, repeat_option, reminder, note_id)

        public ObservableCollection<MyTask> GetAllTasks()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyTask>();
            var sql = @"
                select task.task_id, task.name, task.due_date, task.description, task.completed, 
                       task.important, task.repeat_option, task.reminder, task.note_id
                from [TASK] task
                where task.username = @username
            ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskId = reader.GetInt32(0),
                    TaskName = reader.GetString(1),
                    DueDateTime = reader.GetDateTime(2),
                    Description = reader.GetString(3),
                    IsCompleted = reader.GetBoolean(4),
                    IsImportant = reader.GetBoolean(5),
                    RepeatOption = reader.IsDBNull(6) ? null : reader.GetString(6),
                    ReminderTime = reader.GetDateTime(7),
                    NoteId = reader.IsDBNull(8) ? -1 : reader.GetInt32(8)
                };
                result.Add(task);
            }

            connection.Close();
            return result;
        }

        public ObservableCollection<MyTask> GetTodayTask()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyTask>();
            var sql = @"
                select task.task_id, task.name, task.due_date, task.description, task.completed, 
                       task.important, task.repeat_option, task.reminder, task.note_id
                from [TASK] task
                where task.username = @username 
                and task.due_date >= CAST(GETDATE() AS DATE) 
                and task.due_date < CAST(GETDATE() + 1 AS DATE)
            ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskId = reader.GetInt32(0),
                    TaskName = reader.GetString(1),
                    DueDateTime = reader.GetDateTime(2),
                    Description = reader.GetString(3),
                    IsCompleted = reader.GetBoolean(4),
                    IsImportant = reader.GetBoolean(5),
                    RepeatOption = reader.IsDBNull(6) ? null : reader.GetString(6),
                    ReminderTime = reader.GetDateTime(7),
                    NoteId = reader.IsDBNull(8) ? -1 : reader.GetInt32(8)
                };
                result.Add(task);
            }

            connection.Close();
            return result;
        }

        public void InsertTask(MyTask task)
        {
            var connection = CreateConnection();
            var sql = @"
                insert into [TASK] (name, due_date, description, completed, important, repeat_option, reminder, note_id, username)
                values (@name, @due_date, @description, @completed, @important, @repeat_option, @reminder, @note_id, @username)
            ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", task.TaskName);
            command.Parameters.AddWithValue("@due_date", task.DueDateTime);
            command.Parameters.AddWithValue("@description", task.Description);
            command.Parameters.AddWithValue("@completed", task.IsCompleted);
            command.Parameters.AddWithValue("@important", task.IsImportant);
            command.Parameters.AddWithValue("@repeat_option", task.RepeatOption ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@reminder", task.ReminderTime);
            command.Parameters.AddWithValue("@note_id", task.NoteId == -1 ? (object)DBNull.Value : task.NoteId);
            command.Parameters.AddWithValue("@username", User.Username);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateTask(MyTask task)
        {
            var connection = CreateConnection();
            var sql = @"
                update [TASK] 
                set name = @name, 
                    due_date = @due_date, 
                    description = @description, 
                    completed = @completed, 
                    important = @important, 
                    repeat_option = @repeat_option, 
                    reminder = @reminder, 
                    note_id = @note_id
                where task_id = @task_id and username = @username
            ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@task_id", task.TaskId);
            command.Parameters.AddWithValue("@name", task.TaskName);
            command.Parameters.AddWithValue("@due_date", task.DueDateTime);
            command.Parameters.AddWithValue("@description", task.Description);
            command.Parameters.AddWithValue("@completed", task.IsCompleted);
            command.Parameters.AddWithValue("@important", task.IsImportant);
            command.Parameters.AddWithValue("@repeat_option", task.RepeatOption ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@reminder", task.ReminderTime);
            command.Parameters.AddWithValue("@note_id", task.NoteId == -1 ? (object)DBNull.Value : task.NoteId);
            command.Parameters.AddWithValue("@username", User.Username);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteTask(MyTask task)
        {
            var connection = CreateConnection();
            var sql = @"
                delete from [TASK] 
                where task_id = @task_id and username = @username
            ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@task_id", task.TaskId);
            command.Parameters.AddWithValue("@username", User.Username);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public ObservableCollection<MyTask> GetTasksForDate(DateTime date)
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyTask>();

            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var sql = @"
                select task.task_id, task.name, task.due_date, task.description, task.completed, 
                       task.important, task.repeat_option, task.reminder, task.note_id
                from [TASK] task
                where task.username = @username 
                and task.due_date >= @startDate 
                and task.due_date < @endDate
            ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = User.Username;
            command.Parameters.Add("@startDate", System.Data.SqlDbType.DateTime).Value = startDate;
            command.Parameters.Add("@endDate", System.Data.SqlDbType.DateTime).Value = endDate;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskId = reader.GetInt32(0),
                    TaskName = reader.GetString(1),
                    DueDateTime = reader.GetDateTime(2),
                    Description = reader.GetString(3),
                    IsCompleted = reader.GetBoolean(4),
                    IsImportant = reader.GetBoolean(5),
                    RepeatOption = reader.IsDBNull(6) ? null : reader.GetString(6),
                    ReminderTime = reader.GetDateTime(7),
                    NoteId = reader.IsDBNull(8) ? -1 : reader.GetInt32(8)
                };
                result.Add(task);
            }

            connection.Close();
            return result;
        }

        public ObservableCollection<MyTask> GetRepeatingTasks()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyTask>();

            var sql = @"
                SELECT task.task_id, task.name, task.due_date, task.description, task.completed, 
                       task.important, task.repeat_option, task.reminder, task.note_id
                FROM [TASK] task
                WHERE task.repeat_option IN ('Daily', 'Weekly', 'Monthly');
            ";

            var command = new SqlCommand(sql, connection);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskId = reader.GetInt32(0),
                    TaskName = reader.GetString(1),
                    DueDateTime = reader.GetDateTime(2),
                    Description = reader.GetString(3),
                    IsCompleted = reader.GetBoolean(4),
                    IsImportant = reader.GetBoolean(5),
                    RepeatOption = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                    ReminderTime = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7),
                    NoteId = reader.IsDBNull(8) ? -1 : reader.GetInt32(8)
                };
                result.Add(task);
            }

            connection.Close();
            return result;
        }


        // Timer ------------------------------------------------------------------------------------

        public void SaveSession(Session session)
        {
            using (var connection = CreateConnection())
            {
                var sql = @"
                    INSERT INTO SESSION (username, duration, tag, timestamp, type)
                    VALUES (@username, @duration, @tag, @timestamp, @type)
                ";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@username", User.Username);
                command.Parameters.AddWithValue("@duration", session.Duration); // Store duration as seconds
                command.Parameters.AddWithValue("@tag", session.Tag);
                command.Parameters.AddWithValue("@timestamp", session.Timestamp);
                command.Parameters.AddWithValue("@type", session.Type);
                command.ExecuteNonQuery();
            }
        }

        public List<Session> GetAllSessions()
        {
            var sessions = new List<Session>();
            using (var connection = CreateConnection())
            {
                var sql = @"
                    SELECT session_id, duration, tag, timestamp, type
                    FROM SESSION
                    WHERE username = @username
                ";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@username", User.Username);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sessions.Add(new Session
                    {
                        Id = reader.GetInt32(0),
                        Duration = reader.GetInt32(1), // Retrieve duration as seconds
                        Tag = reader.GetString(2),
                        Timestamp = reader.GetDateTime(3),
                        Type = reader.GetString(4) // Retrieve the type
                    });
                }
            }
            return sessions;
        }

        public List<Session> GetAllSessionsWithTag(string tag)
        {
            var sessions = new List<Session>();
            using (var connection = CreateConnection())
            {
                var sql = @"
                    SELECT session_id, duration, tag, timestamp, type
                    FROM SESSION
                    WHERE username = @username AND tag = @tag
                ";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@username", User.Username);
                command.Parameters.AddWithValue("@tag", tag);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sessions.Add(new Session
                    {
                        Id = reader.GetInt32(0),
                        Duration = reader.GetInt32(1), // Retrieve duration as seconds
                        Tag = reader.GetString(2),
                        Timestamp = reader.GetDateTime(3),
                        Type = reader.GetString(4) // Retrieve the type
                    });
                }
            }
            return sessions;
        }


        // Background ------------------------------------------------------------------------------------
        public void SaveSelectedBackground(LinearGradientBrush selectedBrush)
        {
            throw new NotImplementedException();
        }

        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2)
        {
            throw new NotImplementedException();
        }
    }
}
