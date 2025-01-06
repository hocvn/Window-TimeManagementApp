using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Helper;
using Windows.System;
using System;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// This page is responsible for displaying the details of a note.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        public NoteViewModel ViewModel { get; set; } = new NoteViewModel();

        public NotePage()
        {
            this.InitializeComponent();
            ViewModel.Init();
            MyColorPicker.Color = ((SolidColorBrush)ViewModel.CurrentColor).Color;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Note = e.Parameter as MyNote;
        }

        private async void NotePage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Load(Editor);
            UnsavedSign.Fill = new SolidColorBrush(Colors.Transparent);
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (ViewModel.BackButton_Clicked == true)
            {
                return;
            }
            if (ViewModel.HasUnsavedChanged == false)
            {
                return;
            }
            var result = await Dialog.ShowContent
            (
                this.XamlRoot, 
                "Save".GetLocalized(), 
                "Would_you_like_to_save_the_recent_note".GetLocalized(), 
                "Yes".GetLocalized(), 
                "No".GetLocalized(), 
                null
            );
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Save(Editor);
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.BackButton_Clicked = true;
            if (ViewModel.HasUnsavedChanged == false)
            {
                if (Frame.CanGoBack)
                {
                    MainWindow.NavigationService.GoBack();
                }
                return;
            }

            if (!ViewModel.HasUnsavedChanged)
            {
                if (Frame.CanGoBack)
                {
                    MainWindow.NavigationService.GoBack();
                }
            }

            var result = await Dialog.ShowContent
            (
                this.XamlRoot, 
                "Exit".GetLocalized(), 
                "Would_you_like_to_save".GetLocalized(), 
                "Yes".GetLocalized(), 
                "No".GetLocalized(), 
                "Cancel".GetLocalized()
            );
            if (result == ContentDialogResult.Primary)
            {
                if (Frame.CanGoBack)
                {
                    MainWindow.NavigationService.GoBack();
                }
            }
            else if (result == ContentDialogResult.Secondary)
            {
                if (Frame.CanGoBack)
                {
                    MainWindow.NavigationService.GoBack();
                }
            }
            ViewModel.BackButton_Clicked = false;
        }

        private void CheckChanged()
        {
            Editor.Document.GetText(TextGetOptions.FormatRtf, out string currentContent);
            ViewModel.HasUnsavedChanged = !string.Equals(ViewModel.Note.Content, currentContent, StringComparison.Ordinal);
            if (ViewModel.HasUnsavedChanged)
            {
                UnsavedSign.Fill = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                UnsavedSign.Fill = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Save(Editor);

            Editor.Document.GetText(TextGetOptions.FormatRtf, out string content);
            ViewModel.Note.Content = content;

            ViewModel.HasUnsavedChanged = false;
            UnsavedSign.Fill = new SolidColorBrush(Colors.Transparent);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            var result = await Dialog.ShowContent
            (
                this.XamlRoot, 
                "Delete_note".GetLocalized(), 
                "Are_you_sure_you_want_to_remove_this_note".GetLocalized(), 
                "Yes".GetLocalized(), 
                null, 
                "No".GetLocalized()
            );
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Remove();
            }
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = Editor.Document.Selection;
            selection.CharacterFormat.Underline = selection.CharacterFormat.Underline == UnderlineType.None ? UnderlineType.Single : UnderlineType.None;
        }

        private void MyColorPicker_ColorChanged(object sender, ColorChangedEventArgs args)
        {
            ViewModel.CurrentColor = new SolidColorBrush(args.NewColor);
        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor.Document.GetText(TextGetOptions.UseCrlf, out _);

            ITextRange documentRange = Editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush background = new(Colors.White);


            if (background != null)
            {
                documentRange.CharacterFormat.BackgroundColor = background.Color;
            }
        }

        private void Editor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selection = Editor.Document.Selection;
            if (selection.Length == 0)
            {
                var documentRange = Editor.Document.GetRange(0, TextConstants.MaxUnitCount);
                documentRange.CharacterFormat.BackgroundColor = Colors.Transparent;
            }
        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            Editor.Document.Selection.CharacterFormat.ForegroundColor = ((SolidColorBrush)ViewModel.CurrentColor).Color;
            CheckChanged();
        }

        private void NoteName_LostFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.Rename((TextBox)sender);
        }

        private void NoteNameTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.Rename((TextBox)sender);
                Editor.Focus(FocusState.Programmatic);
                UnsavedSign.Fill = new SolidColorBrush(Colors.Blue); // Mark as unsaved
                e.Handled = true;
            }
        }
    }
}
