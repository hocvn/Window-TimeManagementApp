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
        MyNote Note { get; set; }

        public NotePage()
        {
            this.InitializeComponent();
            Editor.SelectionChanged += Editor_SelectionChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Note = e.Parameter as MyNote;
            IDao dao = new MockDao();
            dao.OpenRtf(Editor, Note);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            IDao dao = new MockDao();
            dao.SaveRtf(Editor, Note);
            Frame.GoBack();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            IDao dao = new MockDao();
            dao.SaveRtf(Editor, Note);
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

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Microsoft.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((SolidColorBrush)rectangle.Fill).Color;

            Editor.Document.Selection.CharacterFormat.ForegroundColor = color;

            FontColorButton.Flyout.Hide();
            Editor.Focus(FocusState.Keyboard);
            currentColor = color;
        }

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
    }
}
