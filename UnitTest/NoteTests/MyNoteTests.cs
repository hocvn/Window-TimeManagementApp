using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Note;
using System;

namespace UnitTest.NoteTests
{
    [TestClass]
    public class MyNoteTests
    {
        [TestMethod]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            // Arrange
            var id = 1;
            var name = "Test Note";
            var content = "This is a test note.";

            // Act
            var note = new MyNote
            {
                Id = id,
                Name = name,
                Content = content
            };

            // Assert
            Assert.AreEqual(id, note.Id);
            Assert.AreEqual(name, note.Name);
            Assert.AreEqual(content, note.Content);
        }
    }
}