using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Helper;
using Windows.System;
using System;
using System.Threading.Tasks;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        private string _originalContent;

        public NoteViewModel ViewModel { get; set; } = new NoteViewModel();

        public NotePage()
        {
            this.InitializeComponent();
            ViewModel.Init();
            MyColorPicker.Color = ((SolidColorBrush)ViewModel.CurrentColor).Color;
            Editor.SelectionChanged += Editor_SelectionChanged;
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
            Editor.Document.GetText(TextGetOptions.None, out _originalContent);
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (ViewModel.BackButton_Clicked == true)
            {
                return;
            }
            if (HasUnsavedChanges() == false)
            {
                return;
            }
            var result = await Dialog.ShowContent(this.XamlRoot, "Save", "Would you like to save the recent note?", "Yes", "No", null);
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Save(Editor);
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.BackButton_Clicked = true;
            // Note has no changes
            if (HasUnsavedChanges() == false)
            {
                if (Frame.CanGoBack)
                {
                    MainWindow.NavigationService.GoBack();
                }
                return;
            }

            var result = await Dialog.ShowContent(this.XamlRoot, "Exit", "Would you like to save?", "Yes", "No", "Cancel");
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

        private bool HasUnsavedChanges()
        {
            Editor.Document.GetText(TextGetOptions.None, out string currentContent);
            return !string.Equals(_originalContent, currentContent, StringComparison.Ordinal);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Save(Editor);
            Editor.Document.GetText(TextGetOptions.None, out _originalContent);
            UnsavedSign.Fill = new SolidColorBrush(Colors.Transparent);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Remove(this.XamlRoot);
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
            // Assign the selected color to a variable to use outside the popup.
            ViewModel.CurrentColor = new SolidColorBrush(MyColorPicker.Color);
        }

        // This function get from winui3 sample
        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor.Document.GetText(TextGetOptions.UseCrlf, out _);

            // reset colors to correct defaults for Focused state
            ITextRange documentRange = Editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush background = new(Colors.White);

            if (background != null)
            {
                documentRange.CharacterFormat.BackgroundColor = background.Color;
            }
        }

        private void Editor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Editor.Document.Selection.Length == 0)
            {
                var documentRange = Editor.Document.GetRange(0, TextConstants.MaxUnitCount);
                documentRange.CharacterFormat.BackgroundColor = Colors.Transparent;
            }
        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            Editor.Document.Selection.CharacterFormat.ForegroundColor = ((SolidColorBrush)ViewModel.CurrentColor).Color;

            if (HasUnsavedChanges() == true)
            {
                UnsavedSign.Fill = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                UnsavedSign.Fill = new SolidColorBrush(Colors.Transparent);
            }
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
                Editor.Focus(FocusState.Programmatic); // Shift focus to RichEditBox
                e.Handled = true; // Mark the event as handled
            }
        }
    }
}
