using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace TimeManagementApp.Helper
{
    public class Dialog
    {
        public static async Task<ContentDialogResult> ShowContent
            (XamlRoot root, string title, string content, string primaryButtonText, string secondaryButtonText, string closeButtonText)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = root,
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText,
            };

            ContentDialogResult result = await contentDialog.ShowAsync();
            return result;
        }
    }
}