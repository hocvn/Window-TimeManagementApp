using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeManagementApp.ToDo;
using System.ComponentModel;
using System;

namespace UnitTest
{
    [TestClass]
    public class MyTaskTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            string taskName = "Test Task";
            string taskDescription = "Test Description";
            DateTime startDateTime = DateTime.Now;
            DateTime dueDateTime = DateTime.Now.AddHours(1);

            // Act
            var task = new MyTask
            {
                TaskName = taskName,
                TaskDescription = taskDescription,
                StartDateTime = startDateTime,
                DueDateTime = dueDateTime
            };

            // Assert
            Assert.AreEqual(taskName, task.TaskName);
            Assert.AreEqual(taskDescription, task.TaskDescription);
            Assert.AreEqual(startDateTime, task.StartDateTime);
            Assert.AreEqual(dueDateTime, task.DueDateTime);
        }

        [TestMethod]
        public void SetProperties_ShouldRaisePropertyChanged()
        {
            // Arrange
            var task = new MyTask();
            bool propertyChangedRaised = false;

            task.PropertyChanged += (sender, args) => {
                if (args.PropertyName == "TaskName")
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            task.TaskName = "New Task";

            // Assert
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void SetProperties_ShouldUpdateValues()
        {
            // Arrange
            var task = new MyTask();
            string newTaskName = "Updated Task";
            string newTaskDescription = "Updated Description";
            DateTime newStartDateTime = DateTime.Now.AddHours(1);
            DateTime newDueDateTime = DateTime.Now.AddHours(2);

            // Act
            task.TaskName = newTaskName;
            task.TaskDescription = newTaskDescription;
            task.StartDateTime = newStartDateTime;
            task.DueDateTime = newDueDateTime;

            // Assert
            Assert.AreEqual(newTaskName, task.TaskName);
            Assert.AreEqual(newTaskDescription, task.TaskDescription);
            Assert.AreEqual(newStartDateTime, task.StartDateTime);
            Assert.AreEqual(newDueDateTime, task.DueDateTime);
        }
    }
}
