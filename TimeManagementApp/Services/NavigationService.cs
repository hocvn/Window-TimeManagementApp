using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Services
{
    public class NavigationService
    {
        private Frame _mainFrame;
        public Type LastNavigatedPage { get; private set; }

        public void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }

        public void Navigate(Type pageType, object parameter = null)
        {
            if (_mainFrame != null && _mainFrame.CurrentSourcePageType != pageType)
            {
                _mainFrame.Navigate(pageType, parameter);
                LastNavigatedPage = pageType;
            }
        }

        public void GoBack()
        {
            if (_mainFrame?.CanGoBack == true)
            {
                _mainFrame.GoBack();
                LastNavigatedPage = _mainFrame.CurrentSourcePageType;
            }
        }
    }

}
