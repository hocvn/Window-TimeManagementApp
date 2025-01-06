using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.ToDo;
using System;

namespace UnitTest.ToDoTests
{
    [TestClass]
    public class MyTaskTests
    {
        [TestMethod]
        public void Clone_WhenCalled_ShouldReturnExactCopy()
        {
            // Arrange
            var originalTask = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task",
                DueDateTime = DateTime.Now,
                Description = "Test Description",
                IsCompleted = false,
                IsImportant = true,
                RepeatOption = "Daily",
                ReminderTime = DateTime.Now.AddHours(1),
                NoteId = 1,
                Status = "Pending"
            };

            // Act
            var clonedTask = (MyTask)originalTask.Clone();

            // Assert
            Assert.IsTrue(MyTask.IsEqual(originalTask, clonedTask));
        }

        [TestMethod]
        public void IsEqual_WhenTasksAreEqual_ShouldReturnTrue()
        {
            // Arrange
            var task1 = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task",
                DueDateTime = DateTime.Now,
                Description = "Test Description",
                IsCompleted = false,
                IsImportant = true,
                RepeatOption = "Daily",
                ReminderTime = DateTime.Now.AddHours(1),
                NoteId = 1,
                Status = "Pending"
            };

            var task2 = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task",
                DueDateTime = DateTime.Now,
                Description = "Test Description",
                IsCompleted = false,
                IsImportant = true,
                RepeatOption = "Daily",
                ReminderTime = DateTime.Now.AddHours(1),
                NoteId = 1,
                Status = "Pending"
            };

            // Act
            var result = MyTask.IsEqual(task1, task2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsEqual_WhenTasksAreNotEqual_ShouldReturnFalse()
        {
            // Arrange
            var task1 = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task",
                DueDateTime = DateTime.Now,
                Description = "Test Description",
                IsCompleted = false,
                IsImportant = true,
                RepeatOption = "Daily",
                ReminderTime = DateTime.Now.AddHours(1),
                NoteId = 1,
                Status = "Pending"
            };

            var task2 = new MyTask
            {
                TaskId = 2,
                TaskName = "Different Task",
                DueDateTime = DateTime.Now.AddDays(1),
                Description = "Different Description",
                IsCompleted = true,
                IsImportant = false,
                RepeatOption = "Weekly",
                ReminderTime = DateTime.Now.AddHours(2),
                NoteId = 2,
                Status = "Completed"
            };

            // Act
            var result = MyTask.IsEqual(task1, task2);

            // Assert
            Assert.IsFalse(result);
        }
    }
}