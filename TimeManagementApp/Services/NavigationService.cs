using Microsoft.UI.Xaml.Controls;
using System;

namespace TimeManagementApp.Services
{
    public class NavigationService
    {
        private Frame _mainFrame;

        public void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }

        public void Navigate(Type pageType, object parameter = null)
        {
            if (_mainFrame != null && _mainFrame.CurrentSourcePageType != pageType)
            {
                _mainFrame.Navigate(pageType, parameter);
            }
        }

        public void GoBack()
        {
            if (_mainFrame?.CanGoBack == true)
            {
                _mainFrame.GoBack();
            }
        }
    }
}