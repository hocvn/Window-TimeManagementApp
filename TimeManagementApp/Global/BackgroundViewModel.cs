using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementApp.Dao;
using Windows.Foundation;

namespace TimeManagementApp.Global
{
    public class BackgroundViewModel : INotifyPropertyChanged
    {
        public LinearGradientBrush PageBackgroundBrush { get; set; }
        public LinearGradientBrush NavigationViewBackgroundBrush { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BackgroundViewModel()
        {
            IDao dao = new LocalSettingsDao();

            PageBackgroundBrush = dao.LoadSavedBackground(0.0, 8.0);
            NavigationViewBackgroundBrush = dao.LoadSavedBackground(0.0, 2.5);

            if (PageBackgroundBrush == null)
            {
                PageBackgroundBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.AliceBlue, Offset = 0.0 },
                        new GradientStop { Color = Colors.CornflowerBlue, Offset = 8.0 }
                    }
                };

                NavigationViewBackgroundBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.AliceBlue, Offset = 0.0 },
                        new GradientStop { Color = Colors.CornflowerBlue, Offset = 2.5 }
                    }
                };
            }
        }
    }
}
