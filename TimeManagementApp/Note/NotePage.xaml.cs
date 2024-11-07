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
using Windows.UI;
using System.ComponentModel;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        private string _originalContentWithFormat;

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
            Editor.Document.GetText(TextGetOptions.FormatRtf, out _originalContentWithFormat);
            Editor.SelectionChanged += Editor_SelectionChanged;
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
            var result = await Dialog.ShowContent(this.XamlRoot, "Save", "Would you like to save the recent note?", "Yes", "No", null);
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

        private void CheckChanged()
        {
            Editor.Document.GetText(TextGetOptions.FormatRtf, out string currentContent);
            ViewModel.HasUnsavedChanged = !string.Equals(_originalContentWithFormat, currentContent, StringComparison.Ordinal);
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
            Editor.Document.GetText(TextGetOptions.FormatRtf, out _originalContentWithFormat);
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
            else
            {
                CheckChanged();
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
                e.Handled = true;
            }
        }
    }
}
