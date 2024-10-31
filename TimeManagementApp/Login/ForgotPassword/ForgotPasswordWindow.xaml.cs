using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            //var name = "EmailPage";
            //var type = typeof(name);
            rootFrame.Navigate(typeof(EmailPage), this);
        }
    }
}
