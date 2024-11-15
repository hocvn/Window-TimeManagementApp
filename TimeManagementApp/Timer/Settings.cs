using System.ComponentModel;

namespace TimeManagementApp.Timer
{
    public class Settings : INotifyPropertyChanged
    {
        public int FocusTimeMinutes { get; set; }
        public int ShortBreakMinutes { get; set; }
        public int LongBreakMinutes { get; set; }

        public bool IsNotificationOn { get; set; }

        public string Tag { get; set; }

        // default settings
        public Settings()
        {
            FocusTimeMinutes = 25;
            ShortBreakMinutes = 5;
            LongBreakMinutes = 10;

            IsNotificationOn = true;

            Tag = "Studying";
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
