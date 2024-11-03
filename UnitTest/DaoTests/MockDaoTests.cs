using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using TimeManagementApp.Dao;
using TimeManagementApp.Note;
using TimeManagementApp.ToDo;
using Windows.Storage;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Text;

namespace UnitTest
{
    [TestClass]
    public class MockDaoTests
    {
        private IDao dao;

        [TestInitialize]
        public void TestInitialize()
        {
            dao = new MockDao();
            ApplicationData.Current.LocalSettings.Values["mynotes"] = null;
        }


        [UITestMethod]
        public void RenameNote_ShouldUpdateNoteName()
        {
            // Arrange
            var note = new MyNote { Id = "1", Name = "Note 1" };
            var notes = new ObservableCollection<MyNote> { note };
            dao.SaveNotes(notes);

            // Act
            note.Name = "Updated Note 1";
            dao.RenameNote(note);

            // Assert
            var result = dao.GetAllNote();
            Assert.AreEqual("Updated Note 1", result.FirstOrDefault(n => n.Id == "1").Name);
        }

        [UITestMethod]
        public void GetAllTasks_ShouldReturnAllTasks()
        {
            // Act
            var result = dao.GetAllTasks();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Task 01", result[0].TaskName);
            Assert.AreEqual("Task 02", result[1].TaskName);
            Assert.AreEqual("Task 03", result[2].TaskName);
        }
    }
}
