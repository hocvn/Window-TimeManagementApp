using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Timer
{
    public class Settings
    {
        public int FocusTimeMinutes { get; set; }
        public int ShortBreakMinutes { get; set; }
        public int LongBreakMinutes { get; set; }

        public Settings()
        {
            FocusTimeMinutes = 25;
            ShortBreakMinutes = 5;
            LongBreakMinutes = 10;
        }
    }

    public enum TimerType
    {
        FocusTime,
        ShortBreak,
        LongBreak,
    }

}
