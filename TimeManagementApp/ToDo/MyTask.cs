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
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime DueDateTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}