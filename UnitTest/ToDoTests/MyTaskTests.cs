using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TimeManagementApp.ToDo;

namespace UnitTest
{
    [TestClass]
    public class MyTaskTests
    {
        [TestMethod]
        public void Clone_ShouldReturnDeepCopyOfTask()
        {
            // Arrange
            var task = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task",
                DueDateTime = DateTime.Now,
                Description = "Test Description",
                IsCompleted = false,
                IsImportant = true,
                RepeatOption = "Daily",
                ReminderTime = DateTime.Now.AddHours(1),
                NoteId = 1
            };

            // Act
            var clonedTask = (MyTask)task.Clone();

            // Assert
            Assert.AreNotSame(task, clonedTask);
            Assert.AreEqual(task.TaskId, clonedTask.TaskId);
            Assert.AreEqual(task.TaskName, clonedTask.TaskName);
            Assert.AreEqual(task.DueDateTime, clonedTask.DueDateTime);
            Assert.AreEqual(task.Description, clonedTask.Description);
            Assert.AreEqual(task.IsCompleted, clonedTask.IsCompleted);
            Assert.AreEqual(task.IsImportant, clonedTask.IsImportant);
            Assert.AreEqual(task.RepeatOption, clonedTask.RepeatOption);
            Assert.AreEqual(task.ReminderTime, clonedTask.ReminderTime);
            Assert.AreEqual(task.NoteId, clonedTask.NoteId);
        }

        [TestMethod]
        public void IsEqual_ShouldReturnTrueForEqualTasks()
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
                NoteId = 1
            };

            var task2 = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task",
                DueDateTime = task1.DueDateTime,
                Description = "Test Description",
                IsCompleted = false,
                IsImportant = true,
                RepeatOption = "Daily",
                ReminderTime = task1.ReminderTime,
                NoteId = 1
            };

            // Act
            var result = MyTask.IsEqual(task1, task2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsEqual_ShouldReturnFalseForDifferentTasks()
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
                NoteId = 1
            };

            var task2 = new MyTask
            {
                TaskId = 2,
                TaskName = "Another Task",
                DueDateTime = DateTime.Now.AddDays(1),
                Description = "Another Description",
                IsCompleted = true,
                IsImportant = false,
                RepeatOption = "Weekly",
                ReminderTime = DateTime.Now.AddHours(2),
                NoteId = 2
            };

            // Act
            var result = MyTask.IsEqual(task1, task2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsEqual_ShouldReturnFalseIfOneTaskIsNull()
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
                NoteId = 1
            };

            MyTask task2 = null;

            // Act
            var result = MyTask.IsEqual(task1, task2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsEqual_ShouldReturnTrueIfBothTasksAreNull()
        {
            // Arrange
            MyTask task1 = null;
            MyTask task2 = null;

            // Act
            var result = MyTask.IsEqual(task1, task2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PropertyChanged_ShouldRaiseEventWhenPropertyChanges()
        {
            // Arrange
            var task = new MyTask
            {
                TaskId = 1,
                TaskName = "Test Task"
            };

            bool eventRaised = false;
            task.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(task.TaskName))
                {
                    eventRaised = true;
                }
            };

            // Act
            task.TaskName = "Updated Task";

            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
