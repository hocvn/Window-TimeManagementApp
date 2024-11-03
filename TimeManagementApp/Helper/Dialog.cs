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
        public static async Task<ContentDialogResult> ShowContent
            (XamlRoot root, string title, string content, string primaryButtonText, string closeButtonText)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = root,
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = closeButtonText,
            };

            ContentDialogResult result = await contentDialog.ShowAsync();
            return result;
        }
    }
}