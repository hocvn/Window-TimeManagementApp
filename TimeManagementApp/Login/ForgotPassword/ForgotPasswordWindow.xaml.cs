using Microsoft.UI.Xaml;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            this.InitializeComponent();
        }
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            forgotPasswordFrame.Navigate(typeof(ForgotPasswordPage1));
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Activate();
        }
    }
}
