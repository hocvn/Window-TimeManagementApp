using ABI.Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
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

        public int SecondLeft => Minutes * 60 + Seconds;
        public int TotalSecond {
            get
            {
                switch (CurrentType)
                {
                    case TimerType.FocusTime:
                        return CurrentSettings.FocusTimeMinutes * 60;
                    case TimerType.ShortBreak:
                        return CurrentSettings.ShortBreakMinutes * 60;
                    default:
                        return CurrentSettings.LongBreakMinutes * 60;
                }
            }
        }

        public int StrokeThickness = 5;

        public DoubleCollection StrokeDashArray
        {
            get
            {
                double adjustedPerimeter = 1.0 * 628 / StrokeThickness;
                double filledPart = adjustedPerimeter * (1 - 1.0 * SecondLeft / TotalSecond);
                double emptyPart = adjustedPerimeter - filledPart;
                return new DoubleCollection { filledPart, emptyPart };
            }
        }

        public Brush StrokeColor
        {
            get
            {
                switch (CurrentType)
                {
                    case TimerType.FocusTime:
                        return new SolidColorBrush(Colors.Red);
                    case TimerType.ShortBreak:
                        return new SolidColorBrush(Colors.Blue);
                    default:
                        return new SolidColorBrush(Colors.Green);
                }
            }
        }

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
            SetTimerType();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (Seconds == 0)
            {
                if (Minutes == 0)
                {
                    if (CurrentSettings.IsNotificationOn == true)
                    {
                        string title = "Pomodoro Completed!";
                        string message = "Time for a break!";

                        if (CurrentType != TimerType.FocusTime)
                        {
                            title = "Break time ended!";
                            message = "Time to focus!";
                        }

                        string toastXml = $@"
                            <toast>
                                <visual>
                                    <binding template='ToastGeneric'>
                                        <text>{title}</text>
                                        <text>{message}</text>
                                    </binding>
                                </visual>
                                <audio src='ms-appx:///Assets/notification.wav'/>
                            </toast>";

                        var toastDoc = new Windows.Data.Xml.Dom.XmlDocument();
                        toastDoc.LoadXml(toastXml);

                        var toast = new Windows.UI.Notifications.ToastNotification(toastDoc);
                        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                    
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
            IsRunning = false;
            Timer.Stop();

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
