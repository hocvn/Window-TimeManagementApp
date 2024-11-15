using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TimeManagementApp.Note;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Dao
{
    public interface IDao
    {
        void CreateUser(string username, string password, string email);

        bool CheckCredential(string username, string password);

        bool IsUsernameInUse(string username);

        bool IsEmailInUse(string username);

        string GetUsername(string email);

        string GetPassword(string username);

        void ResetPassword(string username, string password, string email);

        ObservableCollection<MyNote> GetAllNote();

        void SaveNotes(ObservableCollection<MyNote> notes);

        void SaveNote(RichEditBox editor, MyNote note);

        Task OpenNote(RichEditBox editor, MyNote note);

        void DeleteNote(MyNote note);

        void RenameNote(MyNote note);
      
        ObservableCollection<MyTask> GetAllTasks();

        ObservableCollection<MyTask> GetTodayTask();
    }
}

