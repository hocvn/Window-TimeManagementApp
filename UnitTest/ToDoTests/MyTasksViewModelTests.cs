using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using TimeManagementApp.ToDo;
using TimeManagementApp.Note;
using TimeManagementApp.Dao;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTest
{
    [TestClass]
    public class MyTaskViewModelTests
    {
        private MyTaskViewModel viewModel;
        private IDao dao;

        [TestInitialize]
        public void TestInitialize()
        {
            dao = new MockDao();
            viewModel = new MyTaskViewModel();
        }

        [UITestMethod]
        public void Constructor_ShouldInitializeTasks()
        {
            // Act
            var tasks = viewModel.Tasks;

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(3, tasks.Count); // Assuming MockDao initializes with 3 tasks
        }

        [UITestMethod]
        public void InsertTask_ShouldAddTask()
        {
            // Arrange
            var newTask = new MyTask { TaskName = "New Task", TaskDescription = "New Task Description" };

            // Act
            viewModel.InsertTask(newTask);

            // Assert
            Assert.AreEqual(4, viewModel.Tasks.Count); // Initially 3 tasks + 1 new task
            Assert.IsTrue(viewModel.Tasks.Contains(newTask));
        }

        [UITestMethod]
        public void DeleteTask_ShouldRemoveTask()
        {
            // Arrange
            var taskToRemove = viewModel.Tasks.First();

            // Act
            viewModel.DeleteTask(taskToRemove);

            // Assert
            Assert.AreEqual(2, viewModel.Tasks.Count); // Initially 3 tasks - 1 removed task
            Assert.IsFalse(viewModel.Tasks.Contains(taskToRemove));
        }

        [UITestMethod]
        public void UpdateTask_ShouldModifyTask()
        {
            // Arrange
            var oldTask = viewModel.Tasks.First();
            var updatedTask = new MyTask { TaskName = "Updated Task", TaskDescription = "Updated Description" };

            // Act
            viewModel.UpdateTask(oldTask, updatedTask);

            // Assert
            var task = viewModel.Tasks.First();
            Assert.AreEqual("Updated Task", task.TaskName);
            Assert.AreEqual("Updated Description", task.TaskDescription);
        }
    }
}
