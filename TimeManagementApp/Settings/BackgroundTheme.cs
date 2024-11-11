using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace TimeManagementApp.Settings
{
    public class BackgroundTheme : INotifyPropertyChanged
    {
        private static BackgroundTheme _instance;
        private static readonly object _lock = new object();

        public static BackgroundTheme Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BackgroundTheme();
                    }
                    return _instance;
                }
            }
        }

        public Brush BackgroundBrush { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private BackgroundTheme()
        {
            BackgroundBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop { Color = Colors.AliceBlue, Offset = 0.0 },
                    new GradientStop { Color = Colors.CornflowerBlue, Offset = 8.0 }
                }
            };
        }
    }

}
