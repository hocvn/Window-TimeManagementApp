using CommunityToolkit.WinUI;
using System.ComponentModel;
using TimeManagementApp.Services;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace TimeManagementApp.Music
{
    public class MusicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Hardcoded music titles
        private readonly string[] musicTitle = {
            "SongTitle1".GetLocalized(),
            "SongTitle2".GetLocalized(),
            "SongTitle3".GetLocalized(),
            "SongTitle4".GetLocalized(),
            "SongTitle5".GetLocalized()
        };

        public int CurrentSongIndex { get; set; } = 0;

        public string CurrentSongTitle { get; set; } = "Song 1";

        // Hardcoded animation background paths
        private readonly string[] animatedBackgroundPath = {
            "ms-appx:///Assets/animationBackground/image_animation_loop_1.mp4",
            "ms-appx:///Assets/animationBackground/image_animation_loop_2.mp4",
            "ms-appx:///Assets/animationBackground/image_animation_loop_3.mp4",
        };

        public int CurrentAnimatedBackgroundIndex { get; set; } = 0;

        private readonly MediaPlayer mediaPlayer = new();

        public void Init()
        {
            CurrentSongIndex = 0;
            CurrentSongTitle = musicTitle[0];
            MusicService.SetSongIndex(0);
        }

        public void TogglePlayPause()
        {
            MusicService.ToggleMusic();
        }

        public void SetSongIndex(int songIndex)
        {
            CurrentSongIndex = songIndex;
            CurrentSongTitle = musicTitle[songIndex];
            MusicService.SetSongIndex(songIndex);
        }

        public MediaPlayer GetMediaPlayer()
        {
            return mediaPlayer;
        }

        public void SetupAnimationBackground()
        {
            // Set background animation
            string path = animatedBackgroundPath[CurrentAnimatedBackgroundIndex];
            mediaPlayer.Source = MediaSource.CreateFromUri(new System.Uri(path));
            mediaPlayer.IsLoopingEnabled = true;  // Loop the video
            mediaPlayer.Volume = 0; // Mute the video
        }
    }
}
