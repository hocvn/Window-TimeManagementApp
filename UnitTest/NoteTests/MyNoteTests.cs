using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Note;

namespace UnitTest
{
    [TestClass]
    public class MyNoteTests
    {
        [TestMethod]
        public void MyNote_ConstructorWithParameters_ShouldInitializeProperties()
        {
            // Arrange
            int id = 1;
            string name = "Test Note";

            // Act
            var note = new MyNote(id, name);

            // Assert
            Assert.AreEqual(id, note.Id);
            Assert.AreEqual(name, note.Name);
            Assert.AreEqual(string.Empty, note.Content);
        }

        [TestMethod]
        public void MyNote_DefaultConstructor_ShouldInitializeProperties()
        {
            // Act
            var note = new MyNote();

            // Assert
            Assert.AreEqual(0, note.Id);
            Assert.AreEqual(string.Empty, note.Name);
            Assert.AreEqual(string.Empty, note.Content);
        }

        [TestMethod]
        public void MyNote_SetProperties_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var note = new MyNote();

            // Act
            note.Id = 1;
            note.Name = "Test Note";
            note.Content = "This is the content of the note.";

            // Assert
            Assert.AreEqual(1, note.Id);
            Assert.AreEqual("Test Note", note.Name);
            Assert.AreEqual("This is the content of the note.", note.Content);
        }
    }
}
