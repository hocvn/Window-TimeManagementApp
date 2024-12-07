using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using TimeManagementApp.Settings;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Windows.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTest
{
    [TestClass]
    public class BrushesViewModelTests
    {
        [UITestMethod]
        public void BrushesViewModel_ShouldInitializeBackgroundBrushes()
        {
            // Arrange
            var brushesViewModel = new BrushesViewModel();

            // Act
            var backgroundBrushes = brushesViewModel.BackgroundBrushes;

            // Assert
            Assert.IsNotNull(backgroundBrushes);
            Assert.AreEqual(10, backgroundBrushes.Count); // Assuming there are 10 color pairs in the list
        }

        [UITestMethod]
        public void BrushesViewModel_ShouldContainExpectedBrushes()
        {
            // Arrange
            var brushesViewModel = new BrushesViewModel();
            var expectedColorPairs = new[]
            {
                (StartColor: Colors.AliceBlue, EndColor: Colors.CornflowerBlue),
                (StartColor: Colors.MistyRose, EndColor: Colors.Salmon),
                (StartColor: Colors.LavenderBlush, EndColor: Colors.HotPink),
                (StartColor: Colors.Honeydew, EndColor: Colors.LightGreen),
                (StartColor: Colors.LightYellow, EndColor: Colors.Gold),
                (StartColor: Colors.Lavender, EndColor: Colors.SlateBlue),
                (StartColor: Colors.SeaShell, EndColor: Colors.Chocolate),
                (StartColor: Colors.PapayaWhip, EndColor: Colors.Orange),
                (StartColor: Colors.Aquamarine, EndColor: Colors.Teal),
                (StartColor: Colors.LightGoldenrodYellow, EndColor: Colors.Goldenrod),
                // other themes go here
            };

            // Act
            var backgroundBrushes = brushesViewModel.BackgroundBrushes;

            // Assert
            Assert.AreEqual(expectedColorPairs.Length, backgroundBrushes.Count);
            for (int i = 0; i < expectedColorPairs.Length; i++)
            {
                var brush = backgroundBrushes[i];
                Assert.AreEqual(new Point(0, 0), brush.StartPoint);
                Assert.AreEqual(new Point(0, 1), brush.EndPoint);
                Assert.AreEqual(expectedColorPairs[i].StartColor, brush.GradientStops[0].Color);
                Assert.AreEqual(0.0, brush.GradientStops[0].Offset);
                Assert.AreEqual(expectedColorPairs[i].EndColor, brush.GradientStops[1].Color);
                Assert.AreEqual(1.0, brush.GradientStops[1].Offset);
            }
        }
    }
}
