using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;
using TimeManagementApp.Services;

namespace TimeManagementApp.Account
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentPass { get; set; }

        public string NewPass { get; set; }

        public string ConfirmPass { get; set; }

        public string ErrorMessage { get; set; }

        private IDao _dao;

        public void Init()
        {
            _dao = new SqlDao();
            ErrorMessage = "";
            CurrentPass = "";
            NewPass = "";
            ConfirmPass = "";
        }

        public void SignOut()
        {
            UserSingleton.Instance.SignOut();
            App.NavigateWindow(new LoginWindow());
        }

        public bool CheckPass()
        {
            string username = UserSingleton.Instance.Username;
            string password = CurrentPass;
            return _dao.CheckCredential(username, password);
        } 

        public void ResetPassword()
        {
            string username = UserSingleton.Instance.Username;
            string email = UserSingleton.Instance.Email;  
            _dao.ResetPassword(username, NewPass, email);
            StorageHelper.RemoveSetting("rememberUsername");
        }
    }
}
