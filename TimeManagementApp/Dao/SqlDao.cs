using Microsoft.Data.SqlClient;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.Services;
using TimeManagementApp.Timer;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public class SqlDao : IDao
    {
        public SqlDao() { }

        private readonly UserSingleton User = UserSingleton.Instance;

        private readonly RSAEncryptionService EncryptionService = new();

        private SqlConnection CreateConnection()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "localhost,1433",  
                InitialCatalog = "timemanagementdb",
                UserID = "sa",                  
                Password = "sqlserver@123",       
                TrustServerCertificate = true
            };

            var connectionString = builder.ToString();
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        // User credentials

        public void CreateUser(string username, string password, string email)
        {
            // Encrypt password and save private key to local settings 
            (string EncryptedPasswordBase64, RSAParameters PrivateKey) = EncryptionService.Encrypt(password);
            EncryptionService.SavePrivateKey(PrivateKey, username);

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

                RSAParameters privateKey = EncryptionService.GetPrivateKey(username);
                string decryptedPassword = EncryptionService.Decrypt(encryptedPassword, privateKey);

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
                RSAParameters privateKey = EncryptionService.GetPrivateKey(username);
                string decryptedPassword = EncryptionService.Decrypt(encryptedPassword, privateKey);
                connection.Close();
                return decryptedPassword;
            }
            connection.Close();
            return null;
        }

        public void ResetPassword(string username, string password, string email)
        {
            // Encrypt new password and save private key to local settings 
            (string EncryptedPasswordBase64, RSAParameters PrivateKey) = EncryptionService.Encrypt(password);
            EncryptionService.SavePrivateKey(PrivateKey, username);

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

        // Note
        public ObservableCollection<MyNote> GetAllNote()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyNote>();

            var sql = @"
                        select Note.note_id, Note.name 
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
                    Name = reader.GetString(1)
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
                //editor.Document.SetText(TextSetOptions.FormatRtf, content);
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

        // TASK (task_id, username, name, due_date, description, completed, important, repeat_option, reminder, note_id)
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
                    TaskName = reader.GetString(1),
                    DueDateTime = reader.GetDateTime(2),
                    //TaskDescription = reader.GetString(3),
                    //Completed = reader.GetBoolean(4),
                    //Important = reader.GetBoolean(5),
                    //RepeatOption = reader.GetString(6),
                    //Reminder = reader.GetDateTime(7),
                    //NoteId = reader.Int(8)
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
                        where task.username = @username and CAST(task.due_date AS DATE) = CAST(GETDATE() AS DATE)
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskName = reader.GetString(1),
                    DueDateTime = reader.GetDateTime(2),
                    //TaskDescription = reader.GetString(3),
                    //Completed = reader.GetBoolean(4),
                    //Important = reader.GetBoolean(5),
                    //RepeatOption = reader.GetString(6),
                    //Reminder = reader.GetDateTime(7),
                    //NoteId = reader.GetInt(8)
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
            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = task.TaskName;
            command.Parameters.Add("@due_date", System.Data.SqlDbType.DateTime);
            command.Parameters["@due_date"].Value = task.DueDateTime;
            command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar);
            //command.Parameters["@description"].Value = task.TaskDescription;
            command.Parameters.Add("@completed", System.Data.SqlDbType.Bit);
            command.Parameters["@completed"].Value = false;
            command.Parameters.Add("@important", System.Data.SqlDbType.Bit);
            command.Parameters["@important"].Value = false; // Assuming default value
            command.Parameters.Add("@repeat_option", System.Data.SqlDbType.NVarChar);
            command.Parameters["@repeat_option"].Value = DBNull.Value; // Assuming default value
            command.Parameters.Add("@reminder", System.Data.SqlDbType.DateTime);
            command.Parameters["@reminder"].Value = DBNull.Value; // Assuming default value
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = DBNull.Value; // Assuming default value
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateTask(MyTask task)
        {
            var connection = CreateConnection();
            var sql = @"
                        update [TASK] task
                        set task.name = @name, task.due_date = @due_date, task.description = @description, task.completed = @completed, task.important = @important
                        where task.task_id = @task_id and task.username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = task.TaskName;
            command.Parameters.Add("@due_date", System.Data.SqlDbType.DateTime);
            command.Parameters["@due_date"].Value = task.DueDateTime;
            command.Parameters.Add("@description", System.Data.SqlDbType.NVarChar);
            //command.Parameters["@description"].Value = task.TaskDescription;
            command.Parameters.Add("@completed", System.Data.SqlDbType.Bit);
            //command.Parameters["@completed"].Value = task.Completed;
            command.Parameters.Add("@task_id", System.Data.SqlDbType.Int);
            //command.Parameters["@task_id"].Value = task.Id;
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;
            command.Parameters.Add("@important", System.Data.SqlDbType.Bit);
            //command.Parameters["@important"].Value = task.Important;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteTask(MyTask task)
        {
            var connection = CreateConnection();
            var sql = @"
                        delete from [TASK] task
                        where task.task_id = @task_id and task.username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@task_id", System.Data.SqlDbType.Int);
            //command.Parameters["@task_id"].Value = task.id;
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;
            command.ExecuteNonQuery();
            connection.Close();
        }

        // Time
        public void AddFocusSession(Timer.Settings setting)
        {
            var connection = CreateConnection();
            var sql = @"
                        insert into [FOCUS_SESSION] (timespan, tag, username)
                        values (@timespan, @tag, @username)
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@timespan", System.Data.SqlDbType.BigInt);
            command.Parameters["@timespan"].Value = setting.FocusTimeMinutes;
            command.Parameters.Add("@tag", System.Data.SqlDbType.NVarChar);
            command.Parameters["@tag"].Value = setting.Tag;
            command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar);
            command.Parameters["@username"].Value = User.Username;
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void SaveSession(FocusSession session)
        {
            throw new NotImplementedException();
        }

        public List<FocusSession> GetAllSessions()
        {
            throw new NotImplementedException();
        }

        public List<FocusSession> GetAllSessionsWithTag(string tag)
        {
            throw new NotImplementedException();
        }

        public void SaveSelectedBackground(LinearGradientBrush selectedBrush)
        {
            throw new NotImplementedException();
        }

        public LinearGradientBrush LoadSavedBackground(double offset1, double offset2)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MyTask> GetTasksForDate(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
