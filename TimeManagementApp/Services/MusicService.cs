using System;
using Windows.Media.Playback;

namespace TimeManagementApp.Services
{
    /// <summary>
    /// This class is responsible for managing the music player.
    /// </summary>
    public class MusicService
    {
        public static readonly MediaPlayer mediaPlayer;

        // Hardcoded music paths
        private static readonly string[] musicPaths = {
            "ms-appx:///Assets/songs/chill_song_1.mp3",
            "ms-appx:///Assets/songs/chill_song_2.mp3",
            "ms-appx:///Assets/songs/chill_song_3.mp3",
            "ms-appx:///Assets/songs/chill_song_4.mp3",
            "ms-appx:///Assets/songs/chill_song_5.mp3"
        };

        public static int CurrentSongIndex { get; set; } = 0;

        static MusicService()
        {
            mediaPlayer = new MediaPlayer();
            SetMusic();
        }

        public static MediaPlayer GetMediaPlayer()
        {
            return mediaPlayer;
        }

        public static void SetSongIndex(int songIndex)
        {
            CurrentSongIndex = songIndex;
            SetMusic();
        }

        public static void SetMusic()
        {
            bool isPlaying = GetStatus();

            string musicPath = musicPaths[CurrentSongIndex];
            mediaPlayer.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(musicPath));
            mediaPlayer.Volume = 100;
            mediaPlayer.IsLoopingEnabled = true;

            if (isPlaying)
            {
                mediaPlayer.Play();
            }
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
