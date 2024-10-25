using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Helper
{
    public class Dialog
    {
        public static async Task ShowContent(XamlRoot root, string title, string message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                XamlRoot = root,
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };

            await errorDialog.ShowAsync();
        }
    }
}