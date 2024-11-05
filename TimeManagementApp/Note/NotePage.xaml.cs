using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Navigation;
using TimeManagementApp.Dao;
using System.Diagnostics;
using System;
using System.ComponentModel;
using System.Xml.Linq;
using TimeManagementApp.Helper;

namespace TimeManagementApp.Note
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        public partial class NoteViewModel : INotifyPropertyChanged
        {
            public MyNote Note { get; set; }
            public Brush CurrentColor { get; set; } 

            public event PropertyChangedEventHandler PropertyChanged;
            
            private IDao dao { get; set; } 

            public void Init()
            {
                dao = new MockDao();
                CurrentColor = new SolidColorBrush(Colors.Black);
            }
            public void RenameNote(string newName)
            {
                Note.Name = newName;
                // Update note name in the list
                dao.RenameNote(Note);
            }
        }

        public NoteViewModel ViewModel { get; set; } 
        bool BackButton_Clicked = false;

        public NotePage()
        {
            this.InitializeComponent();
            ViewModel = new NoteViewModel();
            ViewModel.Init();
            Editor.SelectionChanged += Editor_SelectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Note = e.Parameter as MyNote;
        }

        private void NotePage_Loaded(object sender, RoutedEventArgs e)
        {
            IDao dao = new MockDao();
            dao.OpenNote(Editor, ViewModel.Note);
            MyColorPicker.Color = ((SolidColorBrush)ViewModel.CurrentColor).Color;
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (BackButton_Clicked == true)
            {
                return;
            }
            var result = await Dialog.ShowContent(this.XamlRoot, "Save", "Would you like to save the recent note?", "Yes", "No", null);
            if (result == ContentDialogResult.Primary)
            {
                IDao dao = new MockDao();
                dao.SaveNote(Editor, ViewModel.Note);
            }
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Clicked = true;
            var result = await Dialog.ShowContent(this.XamlRoot, "Exit", "Would you like to save?", "Yes", "No", "Cancel");
            if (result == ContentDialogResult.Primary)
            {
                IDao dao = new MockDao();
                dao.SaveNote(Editor, ViewModel.Note);
                Frame.Navigate(typeof(NoteMainPage), null);
            }
            else if (result == ContentDialogResult.Secondary)
            {
                Frame.Navigate(typeof(NoteMainPage), null);
            }
            BackButton_Clicked = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            IDao dao = new MockDao();
            dao.SaveNote(Editor, ViewModel.Note);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await Dialog.ShowContent(this.XamlRoot, "Remove Note", "Are you sure you want to remove this note?", "Yes", null, "No");
            if (result == ContentDialogResult.Primary)
            {
                // Send note back to NoteMainPage to delete
                Frame.Navigate(typeof(NoteMainPage), ViewModel.Note);
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
            // Assign the selected color to a variable to use outside the popup.
            ViewModel.CurrentColor = new SolidColorBrush(MyColorPicker.Color);
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
            Editor.Document.Selection.CharacterFormat.ForegroundColor = ((SolidColorBrush)ViewModel.CurrentColor).Color;
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
                ViewModel.RenameNote(name);
            }
        }
    }
}
