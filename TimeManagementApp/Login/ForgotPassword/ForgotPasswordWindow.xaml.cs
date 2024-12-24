using Microsoft.UI.Xaml;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Login.ForgotPassword
{
    /// <summary>
    /// This window allows the user to reset their password.
    /// </summary>
    public sealed partial class ForgotPasswordWindow : Window
    {
        private bool _isInitialNavigationDone = false;
        
        public ForgotPasswordWindow()
        {
            this.InitializeComponent();
            WindowInitHelper.SetWindowSize(this);
            WindowInitHelper.SetTitle(this, "Time management");
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (!_isInitialNavigationDone)
            {
                rootFrame.Navigate(typeof(EmailPage));
                _isInitialNavigationDone = true;
            }
        }
    }
}
