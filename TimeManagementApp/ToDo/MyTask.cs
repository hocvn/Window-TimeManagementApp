using CommunityToolkit.WinUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.ToDo
{
    public class MyTask : INotifyPropertyChanged, ICloneable
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime DueDateTime { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public string RepeatOption { get; set; }
        public DateTime ReminderTime { get; set; }
        public int NoteId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return new MyTask()
            {
                TaskId = this.TaskId,
                TaskName = this.TaskName,
                DueDateTime = this.DueDateTime,
                Description = this.Description,
                IsCompleted = this.IsCompleted,
                IsImportant = this.IsImportant,
                RepeatOption = this.RepeatOption,
                ReminderTime = this.ReminderTime,
                NoteId = this.NoteId,
            };
        }

        public static bool IsEqual(MyTask t1, MyTask t2)
        {
            if (ReferenceEquals(t1, t2)) return true;
            if (ReferenceEquals(t1, null)) return false;
            if (ReferenceEquals(t2, null)) return false;

            // Round DueDateTime and ReminderTime to the nearest minute for comparison
            // because the TimePicker doesnt have a second selection
            DateTime RoundToNearestMinute(DateTime dateTime)
            {
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            }

            return t1.TaskId == t2.TaskId &&
                   t1.TaskName == t2.TaskName &&
                   RoundToNearestMinute(t1.DueDateTime) == RoundToNearestMinute(t2.DueDateTime) &&
                   t1.Description == t2.Description &&
                   t1.IsCompleted == t2.IsCompleted &&
                   t1.IsImportant == t2.IsImportant &&
                   t1.RepeatOption == t2.RepeatOption &&
                   RoundToNearestMinute(t1.ReminderTime) == RoundToNearestMinute(t2.ReminderTime) &&
                   t1.NoteId == t2.NoteId;
        }

    }
}
