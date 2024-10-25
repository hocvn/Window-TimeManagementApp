using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Timer
{
    public class PomodoroTimer : INotifyPropertyChanged
    {
        private DispatcherTimer Timer { get; set; }
        private bool IsRunning { get; set; }
        private int Minutes { get; set; }
        private int Seconds { get; set; }
        private int CountFocusSessions { get; set; }

        public Settings CurrentSettings { get; set; }
        public TimerType CurrentType { get; set; }
        public string TimerText => $"{Minutes:D2} : {Seconds:D2}";

        public PomodoroTimer(Settings settings, TimerType type)
        {
            CurrentType = type;
            CurrentSettings = settings;

            CountFocusSessions = 0;

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;

            ResetTimer();
        }

        public void StartTimer()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Timer.Start();
            }
        }

        public void PauseTimer()
        {
            if (IsRunning)
            {
                IsRunning = false;
                Timer.Stop();
            }
        }

        public void ResetTimer()
        {
            IsRunning = false;
            Timer.Stop();

            SetTimerType();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (Seconds == 0)
            {
                if (Minutes == 0)
                {
                    Timer.Stop();
                    IsRunning = false;

                    SwitchToNextTimerType();
                    return;
                }

                Minutes--;
                Seconds = 59;
            }
            else
            {
                Seconds--;
            }
        }

        public void SetTimerType()
        {
            switch (CurrentType)
            {
                case TimerType.FocusTime:
                    Minutes = CurrentSettings.FocusTimeMinutes;
                    Seconds = 0;
                    break;
                case TimerType.ShortBreak:
                    Minutes = CurrentSettings.ShortBreakMinutes;
                    Seconds = 0;
                    break;
                case TimerType.LongBreak:
                    Minutes = CurrentSettings.LongBreakMinutes;
                    Seconds = 0;
                    break;
            }
        }

        public void SwitchToNextTimerType()
        {
            if (CurrentType == TimerType.FocusTime)
            {
                CountFocusSessions++;

                if (CountFocusSessions % 4 == 0)
                {
                    CurrentType = TimerType.LongBreak;
                }
                else
                {
                    CurrentType = TimerType.ShortBreak;
                }
            }
            else
            {
                CurrentType = TimerType.FocusTime;
            }

            SetTimerType();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
