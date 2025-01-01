using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Syncfusion.UI.Xaml.Kanban;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TimeManagementApp.Dao;
using TimeManagementApp.ToDo;

namespace TimeManagementApp.Board
{
    public class BoardViewModel
    {
        public ObservableCollection<KanbanModel> TaskDetails { get; set; }

        public BoardViewModel()
        {
            this.TaskDetails = this.GetTaskDetails();
        }

        private ObservableCollection<KanbanModel> GetTaskDetails()
        {
            IDao dao = new SqlDao();
            var tasks = dao.GetAllTasks();

            // Convert to List to sort
            var taskList = new List<MyTask>(tasks);

            // Sort tasks to ensure 'Important' tasks come before 'Normal' tasks
            taskList.Sort((task1, task2) =>
            {
                if (task1.IsImportant && !task2.IsImportant)
                    return -1;
                else if (!task1.IsImportant && task2.IsImportant)
                    return 1;
                else
                    return 0;
            });

            var taskDetails = new ObservableCollection<KanbanModel>();
            KanbanModel taskDetail;

            foreach (var task in taskList)
            {
                taskDetail = new KanbanModel()
                {
                    Id = task.TaskId,
                    Title = task.TaskName,
                    Description = task.Description,
                    Category = DetermineCategory(task),
                    Tags = DetermineTags(task),
                    IndicatorColorKey = DetermineKey(task),
                    Image = new Image
                    {
                        Source = new BitmapImage(new Uri("ms-appx:///Assets/task.png"))
                    }
                };

                taskDetails.Add(taskDetail);
            }

            return taskDetails;
        }

        private string DetermineCategory(MyTask task)
        {
            return task.Status switch
            {
                "Not Started" => "Not Started",
                "In Progress" => "In Progress",
                "Completed" => "Completed",
                "On Hold" => "On Hold",
                _ => "Not Started"
            };
        }

        private List<string> DetermineTags(MyTask task)
        {
            var tags = new List<string>();

            tags.Add("Due: " + task.DueDateTime.ToString("MM/dd/yyyy"));

            if (task.RepeatOption == "Daily" || task.RepeatOption == "Weekly" || task.RepeatOption == "Monthly")
            {
                tags.Add(task.RepeatOption);
            }

            return tags;
        }

        private object DetermineKey(MyTask task)
        {
            if (task.IsImportant)
            {
                return "Important";
            }
            else
            {
                return "Normal";
            }
        }
    }
}