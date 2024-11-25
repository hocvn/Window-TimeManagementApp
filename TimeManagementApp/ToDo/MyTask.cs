using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.ToDo
{
    public class MyTask : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime DueDateTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public string RepeatOption { get; set; }
        public DateTime ReminderTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Summarization => "Binding all properties";
    }
}
