using ABI.Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using Windows.UI.Notifications;

namespace TimeManagementApp.Timer
{
    public class PomodoroTimer : INotifyPropertyChanged
    {
        // singleton
        private static PomodoroTimer _instance;
        private static readonly object _lock = new object();

        public static PomodoroTimer Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PomodoroTimer(new Settings(), TimerType.FocusTime);
                    }
                    return _instance;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        // Basic properties
        public DispatcherTimer Timer { get; set; }
        public bool IsRunning { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int CountFocusSessions { get; set; }

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


        // constructor with settings and type
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


        // Design Timer's stroke
        public int StrokeThickness = 5;

        public DoubleCollection StrokeDashArray
        {
            get
            {
                // 628: perimerter of an circle with diameter 200
                // StrokeDashArray: a double collection of filledPart and emptyPart
                // use for more visualization when timer run

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


        // start, pause, reset the timer
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


        // handle event occurs after a second tick
        private void Timer_Tick(object sender, object e)
        {
            if (Seconds == 0)
            {
                if (Minutes == 0)
                {
                    // timer end
                    if (CurrentSettings.IsNotificationOn == true)
                    {
                        // show notifications toast & sound
                        ShowNotification();
                    }

                    if (CurrentType == TimerType.FocusTime)
                    {
                        // save focus session
                        var session = new FocusSession
                        {
                            Duration = TimeSpan.FromMinutes(CurrentSettings.FocusTimeMinutes),
                            Timestamp = DateTime.UtcNow,
                            Tag = CurrentSettings.Tag,
                        };


                        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                        var filePath = Path.Combine(directory.FullName, "sessions.xlsx");
                        IDao dao = new ExcelDao(filePath);

                        dao.SaveSession(session);
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

        public void SwitchToNextTimerType()
        {
            if (CurrentType == TimerType.FocusTime)
            {
                CountFocusSessions++;

                // normal pomodoro timer has longbreak after three shortbreaks
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


        private void ShowNotification()
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

            var notification = new AppNotification(toastDoc.GetXml());
            AppNotificationManager.Default.Show(notification);
        }


        public int TagComboBoxIndex
        {
            get
            {
                switch (CurrentSettings.Tag)
                {
                    case "Working":
                        return 0;
                    case "Studying":
                        return 1;
                    default:
                        return 2;
                }
            }
        }
    }
}
