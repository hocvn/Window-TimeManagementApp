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
            FocusTimeMinutes = 2;
            ShortBreakMinutes = 3;
            LongBreakMinutes = 5;
        }
    }

    public enum TimerType
    {
        FocusTime,
        ShortBreak,
        LongBreak,
    }

}
