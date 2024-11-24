using System;
using System.ComponentModel;

namespace TimeManagementApp.ToDo
{
    public class MyTask : INotifyPropertyChanged
    {
        public string TaskName { get; set; }
        public string Summarization { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime DueDateTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}