using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Timer;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;

namespace UnitTest.TimerTests
{
    [TestClass]
    public class SessionTests
    {
        [UITestMethod]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = 1;
            var duration = 1500; // 25 minutes in seconds
            var timestamp = DateTime.Now;
            var tag = "Work";
            var type = "Focus";

            // Act
            var session = new Session
            {
                Id = id,
                Duration = duration,
                Timestamp = timestamp,
                Tag = tag,
                Type = type
            };

            // Assert
            Assert.AreEqual(id, session.Id);
            Assert.AreEqual(duration, session.Duration);
            Assert.AreEqual(timestamp, session.Timestamp);
            Assert.AreEqual(tag, session.Tag);
            Assert.AreEqual(type, session.Type);
        }

        [UITestMethod]
        public void Type_SetToFocus_ShouldBeFocus()
        {
            // Arrange
            var session = new Session { Type = "Focus" };

            // Act & Assert
            Assert.AreEqual("Focus", session.Type);
        }

        [UITestMethod]
        public void Type_SetToBreak_ShouldBeBreak()
        {
            // Arrange
            var session = new Session { Type = "Break" };

            // Act & Assert
            Assert.AreEqual("Break", session.Type);
        }

        [UITestMethod]
        public void Tag_SetToNull_ShouldBeNull()
        {
            // Arrange
            var session = new Session { Tag = null };

            // Act & Assert
            Assert.IsNull(session.Tag);
        }
    }
}