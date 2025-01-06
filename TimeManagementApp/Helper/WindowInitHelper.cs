using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;

namespace TimeManagementApp.Helper
{
    /// <summary>
    /// This class is used to initialize the window size, position, and title.
    /// </summary>
    internal class WindowInitHelper
    {
        internal static void SetWindowSize(Window window)
        {
            var appWindow = window.AppWindow; // Get the AppWindow instance from the Window object
            var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);
            var screenWidth = displayArea.WorkArea.Width;
            var screenHeight = displayArea.WorkArea.Height;

            int width = (int)(screenWidth * 0.8);
            int height = (int)(screenHeight * 0.8);

            // Center the window
            int middleX = (int)(screenWidth - width) / 2;
            int middleY = (int)(screenHeight - height) / 2;

            appWindow.MoveAndResize(new Windows.Graphics.RectInt32(middleX, Math.Max(middleY - (int)(height / 18), 0), width, height));
        }

        internal static void SetTitle(Window window, string title)
        {
            window.Title = title; 
        }
    }
}
