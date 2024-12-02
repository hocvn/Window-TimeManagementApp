using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeManagementApp.ToDo;
using TimeManagementApp.Dao;
using System.IO;
using System.Threading.Tasks;

namespace TimeManagementApp.BackgroundTasks
{
    public sealed class ReminderTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            // Load tasks and check reminders
            var tasks = LoadTasks();
            foreach (var task in tasks)
            {
                if (task.ReminderTime <= DateTime.Now &&
                    DateTime.Now.Subtract(task.ReminderTime).TotalMinutes <= 15 &&
                    !task.IsCompleted)
                {
                    ShowToastNotification(task);
                }
            }

            deferral.Complete();
        }

        private IEnumerable<MyTask> LoadTasks()
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); // bin\x64\Debug\net9.0-windows10.0.22621.0\win-x64\AppX\
            var filePath = Path.Combine(baseDirectory.FullName, "..", "..", "..", "..", "..", "..", "Dao", "tasks.xlsx");
            IDao dao = new MockDao(filePath);

            return dao.GetAllTasks();
        }

        private void ShowToastNotification(MyTask task)
        {
            string title = "Task Reminder";
            string message = $"It's time to: {task.TaskName}";

            string toastXml = $@"
                <toast>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>{title}</text>
                            <text>{message}</text>
                        </binding>
                    </visual>
                    <audio src='ms-appx:///Assets/notification.wav'/>
                </toast>";

            var toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            var toastNotification = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

    }
}
