using Microsoft.Data.SqlClient;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using TimeManagementApp.Helper;
using TimeManagementApp.Note;
using TimeManagementApp.Services;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public class SqlDao : IDao
    {
        public SqlDao() { }

        private readonly UserSingleton User = UserSingleton.Instance;

        private readonly EncryptionService EncryptionService = new();

        private SqlConnection CreateConnection()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ".\\SQLEXPRESS",
                InitialCatalog = "TimeManagementDB",
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };

            var connectionString = builder.ToString();
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        // User credentials
        public bool CheckUser(string username, string password)
        {
            var connection = CreateConnection();
            var result = false;
            var sql = @"
                        select user.user_id, user.username, user.encrypted_password
                        from [USER] user
                        where user.username = @username
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters.Add("@password", System.Data.SqlDbType.VarChar);
            command.Parameters["@password"].Value = password;

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                User.UserId = reader.GetInt32(0);
                User.Username = reader.GetString(1);
                User.EncryptedPassword = reader.GetString(2);
            }
            else
            {
                return false;
            }

            string privateKeyJson = StorageHelper.GetSetting(username);
            RSAParameters privateKey = JsonSerializer.Deserialize<RSAParameters>(privateKeyJson);
            // Decrypt password
            string decryptedPassword = EncryptionService.Decrypt(User.EncryptedPassword, privateKey);

            result = (password == decryptedPassword);
            connection.Close();
            return result;
        }

        public void CreateUser(string username, string password, string email)
        {
            // Encrypt password and save private key to local settings 
            (string EncryptedPasswordBase64, RSAParameters PrivateKey) = EncryptionService.Encrypt(password);
            string privateKeyJson = JsonSerializer.Serialize(PrivateKey);
            StorageHelper.SaveSetting(username, privateKeyJson);

            var connection = CreateConnection();
            var sql = @"
                        insert into [USER] (username, password, email)
                        values (@username, @password, @email)
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar);
            command.Parameters["@username"].Value = username;
            command.Parameters.Add("@password", System.Data.SqlDbType.Text);
            command.Parameters["@password"].Value = EncryptedPasswordBase64;
            command.Parameters.Add("@email", System.Data.SqlDbType.VarChar);
            command.Parameters["@email"].Value = email;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ResetPassword(string username, string password, string email)
        {
            throw new NotImplementedException();
        }

        // Note
        public ObservableCollection<MyNote> GetAllNote()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyNote>();
            connection.Open();

            var sql = @"
                        select note.note_id, note.name 
                        from [NOTE] note
                        where note.user_id = @user_id
                    ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var note = new MyNote
                {
                    Id = reader.GetString(0),
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
                        delete from [NOTE] note
                        where note.note_id = @note_id and note.user_id = @user_id
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;
            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public async Task OpenNote(RichEditBox editor, MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        select notenote.content
                        from [NOTE] note
                        where note.note_id = @note_id and note.user_id = @user_id
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;
            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            var reader = await command.ExecuteReaderAsync();
            if (reader.Read())
            {
                var content = reader.GetString(0);
                editor.Document.SetText(TextSetOptions.FormatRtf, content);
            }
            connection.Close();
        }

        public void RenameNote(MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        update [NOTE]
                        set name = @name
                        where note_id = @note_id and user_id = @user_id
                    ";
            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = note.Name;
            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;
            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void SaveNote(RichEditBox editor, MyNote note)
        {
            var connection = CreateConnection();
            var sql = @"
                        update [NOTE] note
                        set note.name = @name, note.content = @content
                        where note_id = @note_id and user_id = @user_id
                    ";
            var command = new SqlCommand(sql, connection);
            // Get content of RichEditBox with RTF format
            command.Parameters.Add("@content", System.Data.SqlDbType.Text);
            editor.Document.GetText(TextGetOptions.FormatRtf, out string content);
            command.Parameters["@content"].Value = content;

            command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
            command.Parameters["@name"].Value = note.Name;

            command.Parameters.Add("@note_id", System.Data.SqlDbType.Int);
            command.Parameters["@note_id"].Value = note.Id;

            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void SaveNotes(ObservableCollection<MyNote> notes)
        {
            var connection = CreateConnection();
            var sql = @"
                        update [NOTE] note
                        set note.name = @name
                        where note.note_id = @note_id and note.user_id = @user_id
                    ";
            foreach (var note in notes)
            {
                var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
                command.Parameters["@name"].Value = note.Name;
                command.Parameters.Add("@note_id", System.Data.SqlDbType.Char);
                command.Parameters["@note_id"].Value = note.Id;
                command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
                command.Parameters["@user_id"].Value = User.UserId;

                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        // Todo-list
        public ObservableCollection<MyTask> GetAllTasks()
        {
            var connection = CreateConnection();
            var result = new ObservableCollection<MyTask>();
            var sql = @"
                        select task.name, task.due_date, task.description, task.completed
                        from [TASK] task
                        where task.user_id = @user_id
                    ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskName = reader.GetString(0),
                    DueDateTime = reader.GetDateTime(1),
                    TaskDescription = reader.GetString(2),
                    //Completed = reader.GetBoolean(3)
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
                        select task.name, task.due_date, task.description, task.completed
                        from [TASK] task
                        where task.user_id = @user_id and CAST(task.due_date AS DATE) = CAST(GETDATE() AS DATE)
                    ";

            var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@user_id", System.Data.SqlDbType.Int);
            command.Parameters["@user_id"].Value = User.UserId;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new MyTask
                {
                    TaskName = reader.GetString(0),
                    DueDateTime = reader.GetDateTime(1),
                    TaskDescription = reader.GetString(2),
                    //Completed = reader.GetBoolean(3)
                };
                result.Add(task);
            }

            connection.Close();
            return result;
        }
    }
}
