using System;
using Microsoft.UI.Xaml;
using Windows.Storage;
using TimeManagementApp.Login.ForgotPassword;
using Microsoft.UI.Windowing;

namespace TimeManagementApp
{
    /// <summary>
    /// Login window uses to resolve operations about sign in, register.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private Window m_window;
        UserCredential user = new UserCredential();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public LoginWindow()
        {
            this.InitializeComponent();

            var rememberUsername = localSettings.Values["rememberUsername"] as string;
            if (!String.IsNullOrEmpty(rememberUsername))
            {
                // Automatically fill in the username and password
                usernameTextBox.Text = rememberUsername;
                var rememberPassword = user.GetPassword(rememberUsername);

                if (rememberPassword == null)
                {
                    // The password is not stored
                    return;
                }
                passwordBox.Password = rememberPassword;
                rememberCheckBox.IsChecked = true;
            }

            // Set the window size
            SetWindowSize();
        }

        private void SetWindowSize()
        {
            var displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
            var screenWidth = displayArea.WorkArea.Width;
            var screenHeight = displayArea.WorkArea.Height;

            int width = (int)(screenWidth * 0.8);
            int height = (int)(screenHeight * 0.8);

            // Center the window
            int middleX = (int)(screenWidth - width) / 2;
            int middleY = (int)(screenHeight - height) / 2;

            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(middleX, Math.Max(middleY - 100, 0), width, height));
        }


        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                errorMessage.Text = "Please enter both username and password.";
                return;
            }
            if (user.CheckCredentials(username, password) == false)
            {
                errorMessage.Text = "Invalid username or password.";
                return;
            }
            // Username and password are correct
            errorMessage.Text = "";
            m_window = new MainWindow();
            m_window.Activate();
            this.Close();
        }

        private void rememberCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var username = usernameTextBox.Text;
            localSettings.Values["rememberUsername"] = username;
        }

        private void rememberCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Remove the stored username
            localSettings.Values.Remove("rememberUsername");
        }

        // Hide error message when user starts typing
        private void usernameTextBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }

        private void passwordBox_Focus(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
        }
        private void forgotPasswordHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            m_window = new ForgotPasswordWindow();
            m_window.Activate();
            this.Close();
            // This feature is not working for now
        }

        private void registerHyperLinkButton_Click(object sender, RoutedEventArgs e)
        {
            m_window = new RegisterWindow();
            m_window.Activate();
            this.Close();
        }
    }

}
