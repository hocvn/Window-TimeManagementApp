using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.ApplicationModel;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordPage1 : Page
    {
        Window _window = null;
        UserCredential user = new UserCredential();
        public ForgotPasswordPage1()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _window = e.Parameter as Window;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Activate();

            _window.Close();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text as string;
            (bool isOk, string mess) = user.CheckEmailFormat(email);

            if (!isOk)
            {
                errorMessage.Text = mess;
                return;
            }

            if (!user.IsEmailInUse(email))
            {
                errorMessage.Text = "Email you entered have not registered yet.";
                return;
            }
            Frame.Navigate(typeof(ForgotPasswordPage2), email);
        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
    }
}
