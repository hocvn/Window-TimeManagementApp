using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Music;
using TimeManagementApp.Services;
using Windows.Media.Playback;
using Moq;

namespace UnitTest.MusicTests
{
    [TestClass]
    public class MusicViewModelTests
    {
        private MusicViewModel _viewModel;
        private Mock<MusicService> _mockMusicService;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new MusicViewModel();
            _mockMusicService = new Mock<MusicService>();
        }

        [TestMethod]
        public void Init_ShouldInitializeCurrentSong()
        {
            // Act
            _viewModel.Init();

            // Assert
            Assert.AreEqual(0, _viewModel.CurrentSongIndex);
            Assert.AreEqual("Song 1", _viewModel.CurrentSongTitle);
        }

        [TestMethod]
        public void SetSongIndex_ShouldUpdateCurrentSong()
        {
            // Act
            _viewModel.SetSongIndex(2);

            // Assert
            Assert.AreEqual(2, _viewModel.CurrentSongIndex);
            Assert.AreEqual("Song 3", _viewModel.CurrentSongTitle);
        }

        [TestMethod]
        public void GetMediaPlayer_ShouldReturnMediaPlayerInstance()
        {
            // Act
            var mediaPlayer = _viewModel.GetMediaPlayer();

            // Assert
            Assert.IsNotNull(mediaPlayer);
            Assert.IsInstanceOfType(mediaPlayer, typeof(MediaPlayer));
        }

        [TestMethod]
        public void SetupAnimationBackground_ShouldSetMediaPlayerSource()
        {
            // Act
            _viewModel.SetupAnimationBackground();

            // Assert
            var mediaPlayer = _viewModel.GetMediaPlayer();
            Assert.IsNotNull(mediaPlayer.Source);
            Assert.IsTrue(mediaPlayer.IsLoopingEnabled);
            Assert.AreEqual(0, mediaPlayer.Volume);
        }
    }
}