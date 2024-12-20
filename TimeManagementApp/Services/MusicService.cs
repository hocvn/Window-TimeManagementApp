using System;
using Windows.Media.Playback;

namespace TimeManagementApp.Services
{
    public class MusicService
    {
        public static readonly MediaPlayer mediaPlayer;

        // Hardcoded music paths
        private static readonly string[] musicPaths = {
            "ms-appx:///Assets/music/chill_song_1.mp3",
            "ms-appx:///Assets/music/chill_song_2.mp3",
            "ms-appx:///Assets/music/chill_song_3.mp3",
            "ms-appx:///Assets/music/chill_song_4.mp3",
            "ms-appx:///Assets/music/chill_song_5.mp3"
        };

        static MusicService()
        {
            mediaPlayer = new MediaPlayer();
            SetMusic(0);

        }

        public static MediaPlayer GetMediaPlayer()
        {
            return mediaPlayer;
        }

        public static void SetMusic(int songIndex)
        {
            string musicPath = musicPaths[songIndex];
            mediaPlayer.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(musicPath));
            mediaPlayer.Volume = 100;
            mediaPlayer.IsLoopingEnabled = true;
        }

        public static bool GetStatus()
        {
            return mediaPlayer.PlaybackSession.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Playing;
        }

        public static void ToggleMusic()
        {
            if (GetStatus())
            {
                mediaPlayer.Pause();
            }
            else
            {
                mediaPlayer.Play();
            }
        }

        public void PlayMusic()
        {
            mediaPlayer.Play();
        }
        public void PauseMusic()
        {
            mediaPlayer.Pause();
        }
    }
}
