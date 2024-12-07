using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TimeManagementApp.Timer;

namespace UnitTest
{
    [TestClass]
    public class FocusSessionTests
    {
        [TestMethod]
        public void FocusSession_ShouldInitializeProperties()
        {
            // Arrange
            var id = 1;
            var duration = TimeSpan.FromMinutes(25);
            var timestamp = DateTime.Now;
            var tag = "Work";

            // Act
            var focusSession = new FocusSession
            {
                Id = id,
                Duration = duration,
                Timestamp = timestamp,
                Tag = tag
            };

            // Assert
            Assert.AreEqual(id, focusSession.Id);
            Assert.AreEqual(duration, focusSession.Duration);
            Assert.AreEqual(timestamp, focusSession.Timestamp);
            Assert.AreEqual(tag, focusSession.Tag);
        }

        [TestMethod]
        public void FocusSession_DefaultConstructor_ShouldInitializePropertiesWithDefaults()
        {
            // Act
            var focusSession = new FocusSession();

            // Assert
            Assert.AreEqual(0, focusSession.Id);
            Assert.AreEqual(default(TimeSpan), focusSession.Duration);
            Assert.AreEqual(default(DateTime), focusSession.Timestamp);
            Assert.IsNull(focusSession.Tag);
        }

        [TestMethod]
        public void FocusSession_SetProperties_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var focusSession = new FocusSession();

            // Act
            focusSession.Id = 2;
            focusSession.Duration = TimeSpan.FromMinutes(30);
            var expectedTimestamp = DateTime.Now.AddMinutes(-30);
            focusSession.Timestamp = expectedTimestamp;
            focusSession.Tag = "Study";

            // Assert
            Assert.AreEqual(2, focusSession.Id);
            Assert.AreEqual(TimeSpan.FromMinutes(30), focusSession.Duration);
            // Use Assert.IsTrue with tolerance for DateTime comparison
            Assert.IsTrue((focusSession.Timestamp - expectedTimestamp).Duration() < TimeSpan.FromSeconds(1), "Timestamp is not within the expected range");
            Assert.AreEqual("Study", focusSession.Tag);
        }
    }
}
