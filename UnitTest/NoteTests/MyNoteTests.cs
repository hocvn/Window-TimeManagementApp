using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.Note;

namespace UnitTest
{
    [TestClass]
    public class MyNoteTests
    {
        [TestMethod]
        public void Constructor_WithParameters_ShouldInitializeProperties()
        {
            // Arrange
            string id = "123";
            string name = "Test Note";

            // Act
            var note = new MyNote(id, name);

            // Assert
            Assert.AreEqual(id, note.Id);
            Assert.AreEqual(name, note.Name);
        }

        [TestMethod]
        public void Constructor_WithoutParameters_ShouldInitializePropertiesToEmpty()
        {
            // Act
            var note = new MyNote();

            // Assert
            Assert.AreEqual("", note.Id);
            Assert.AreEqual("", note.Name);
        }

        [TestMethod]
        public void SetProperties_ShouldUpdateValues()
        {
            // Arrange
            var note = new MyNote();
            string newId = "456";
            string newName = "Updated Note";

            // Act
            note.Id = newId;
            note.Name = newName;

            // Assert
            Assert.AreEqual(newId, note.Id);
            Assert.AreEqual(newName, note.Name);
        }
    }
}
