using Microsoft.UI.Xaml;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// This window allows the user to reset their password.
    /// </summary>
    public sealed partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            this.InitializeComponent();
            this.Title = "Time management";
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            rootFrame.Navigate(typeof(EmailPage));
        }
    }
}
