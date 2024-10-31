using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Dao;
using System.Diagnostics;
using System;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        private Windows.UI.Color currentColor = Colors.Black;

        public class NoteViewModel
        {
            public MyNote Note { get; set; }
        }

        public NoteViewModel ViewModel = new NoteViewModel();

        public NotePage()
        {
            this.InitializeComponent();
            Editor.SelectionChanged += Editor_SelectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Note = e.Parameter as MyNote;
            IDao dao = new MockDao();
            dao.OpenNote(Editor, ViewModel.Note);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            IDao dao = new MockDao();
            dao.SaveNote(Editor, ViewModel.Note);
            Frame.GoBack();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            IDao dao = new MockDao();
            dao.SaveNote(Editor, ViewModel.Note);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Delete Note",
                Content = "Are you sure you want to delete this note?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // Send note back to NoteMainPage to delete
                Frame.Navigate(typeof(NoteMainPage), ViewModel.Note);
            }
        }

        // This function get from winui3 sample
        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        // This function get from winui3 sample
        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = Editor.Document.Selection;
            selection.CharacterFormat.Underline = selection.CharacterFormat.Underline == UnderlineType.None ? UnderlineType.Single : UnderlineType.None;
        }

        // This function get from winui3 sample
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Microsoft.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((SolidColorBrush)rectangle.Fill).Color;

            Editor.Document.Selection.CharacterFormat.ForegroundColor = color;
            RecColor.Fill = new SolidColorBrush(color);

            FontColorButton.Flyout.Hide();
            Editor.Focus(FocusState.Keyboard);
            currentColor = color;
        }

        // This function get from winui3 sample
        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor.Document.GetText(TextGetOptions.UseCrlf, out _);

            // reset colors to correct defaults for Focused state
            ITextRange documentRange = Editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush background = new (Microsoft.UI.Colors.White);

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
            if (Editor.Document.Selection.CharacterFormat.ForegroundColor != currentColor)
            {
                Editor.Document.Selection.CharacterFormat.ForegroundColor = currentColor;
            }
        }

        private void NoteName_LostFocus(object sender, RoutedEventArgs e)
        {
            // Update note if note name is changed
            string name = NoteNameTextBox.Text;

            // If note name is empty, set it back to the original name
            if (name.Length == 0 ) {
                NoteNameTextBox.Text = ViewModel.Note.Name;
                return;
            }

            if (name != ViewModel.Note.Name)
            {
                ViewModel.Note.Name = name;
                // Update note name in the list
                IDao dao = new MockDao();
                dao.RenameNote(ViewModel.Note);
            }
        }
    }
}
