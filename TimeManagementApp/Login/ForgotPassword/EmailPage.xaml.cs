using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.ComponentModel;
using TimeManagementApp.Dao;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// This page requires user to enter email to get OTP
    /// </summary>
    public sealed partial class EmailPage : Page
    {
        public partial class EmailViewModel : INotifyPropertyChanged
        {
            public string ErrorMessage { get; set; }
            private IDao dao { get; set; }

            public EmailViewModel()
            {
                dao = new SqlDao();
                ErrorMessage = "";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public bool IsEmailInUse(string email)
            {
                return dao.IsEmailInUse(email);
            }
        }
        
        public EmailViewModel ViewModel = new();
        public EmailPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateWindow(new LoginWindow());
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text as string;
            (bool isOk, string mess) = CheckingFormatHelper.CheckEmailFormat(email);

            if (!isOk)
            {
                ViewModel.ErrorMessage = mess;
                return;
            }

            if (!ViewModel.IsEmailInUse(email))
            {
                ViewModel.ErrorMessage= "Email_you_entered_have_not_registered_yet".GetLocalized();
                return;
            }
            Frame.Navigate(typeof(OTPPage), email);
        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.ErrorMessage = "";
        }
    }
}
