using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TimeManagementApp.Services;


namespace TimeManagementApp.Music
{
    /// <summary>
    /// This page uses to play lofi music in the background
    /// </summary>
    public sealed partial class MusicPage : Page
    {
        public MusicViewModel ViewModel { get; set; } = new MusicViewModel();
        public MusicPage()
        {
            this.InitializeComponent();
            SetAnimationBackground();
            AnimationBackground.SelectedIndex = ViewModel.currentAnimatedBackgroundIndex;
            Music.SelectedIndex = ViewModel.CurrentSongIndex;
        }

        private void SetAnimationBackground()
        {
            // Set background animation
            ViewModel.SetupAnimationBackground();
            BackgroundVideo.SetMediaPlayer(ViewModel.GetMediaPlayer());
            ViewModel.GetMediaPlayer().Play();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TogglePlayPause();
            if (MusicService.GetStatus())
            {
                PlayPauseIcon.Glyph = "\uF8AE";
            }
            else
            {
                PlayPauseIcon.Glyph = "\uF5B0";
            }
        }

        private void AnimationBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex != ViewModel.currentAnimatedBackgroundIndex)
            {
                ViewModel.currentAnimatedBackgroundIndex = comboBox.SelectedIndex;
                SetAnimationBackground();
            }
        }

        private void Music_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex != ViewModel.CurrentSongIndex)
            {
                ViewModel.SetSongIndex(comboBox.SelectedIndex);
            }
        }
    }
}
