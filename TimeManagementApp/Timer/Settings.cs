using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Timer
{
    public class Settings : INotifyPropertyChanged
    {
        public int FocusTimeMinutes { get; set; }
        public int ShortBreakMinutes { get; set; }
        public int LongBreakMinutes { get; set; }

        public bool IsNotificationOn { get; set; }

        public Settings()
        {
            FocusTimeMinutes = 2;
            ShortBreakMinutes = 3;
            LongBreakMinutes = 5;

            IsNotificationOn = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum TimerType
    {
        FocusTime,
        ShortBreak,
        LongBreak,
    }
}
