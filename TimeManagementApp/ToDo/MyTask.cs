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
    }
}
