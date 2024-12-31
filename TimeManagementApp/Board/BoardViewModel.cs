using Syncfusion.UI.Xaml.Kanban;
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

        /// <summary>
        /// Method to get the kanban model collections.
        /// </summary>
        /// <returns>The kanban model collections.</returns>
        private ObservableCollection<KanbanModel> GetTaskDetails()
        {
            IDao dao = new SqlDao();
            var tasks = dao.GetAllTasks();

            var taskDetails = new ObservableCollection<KanbanModel>();
            KanbanModel taskDetail;

            foreach (var task in tasks)
            {
                taskDetail = new KanbanModel()
                {
                    Title = task.TaskName,
                    Description = task.Description,
                    Category = DetermineCategory(task),
                    Tags = DetermineTags(task)
                };

                taskDetails.Add(taskDetail);
            }

            return taskDetails;
        }

        /// <summary>
        /// Determines the category of the task based on its status.
        /// </summary>
        /// <param name="task">The task to determine the category for.</param>
        /// <returns>The category of the task.</returns>
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

        /// <summary>
        /// Determines the tags for the task.
        /// </summary>
        /// <param name="task">The task to determine the tags for.</param>
        /// <returns>The list of tags for the task.</returns>
        private List<string> DetermineTags(MyTask task)
        {
            var tags = new List<string>();

            if (task.IsImportant)
            {
                tags.Add("Important");
            }

            // Add more tags as needed

            return tags;
        }
    }
}